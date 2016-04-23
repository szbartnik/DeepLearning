using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Wkiro.ImageClassification.Core.Facades;
using Wkiro.ImageClassification.Core.Infrastructure.Logging;
using Wkiro.ImageClassification.Core.Models.Configurations;
using Wkiro.ImageClassification.Core.Models.Dto;
using Wkiro.ImageClassification.Gui.Configuration;
using Wkiro.ImageClassification.Gui.Infrastructure;
using Wkiro.ImageClassification.Gui.Models;
using Application = System.Windows.Application;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace Wkiro.ImageClassification.Gui.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase, ILogger
    {
        private const string SavedNetworkFileExtension = "dbn";

        private ClassifierFacade _classifierFacade;

        private readonly IConfigurationManager _configurationManager;
        private CancellationTokenSource _cts;

        public MainWindowViewModel(bool isNotDesignMode)
        {
            _configurationManager = new SettingsConfigurationManager();

            ProgramState = ProgramState.Initial;

            InitializeCommands();
            DataProviderConfiguration = _configurationManager.GetDataProviderConfiguration();
        }

        private void ConfigureNewTraining()
        {
            GlobalTrainerConfiguration = _configurationManager.GetGlobalTrainerConfiguration();
            Training1Parameters = _configurationManager.GetTraining1Parameters();
            Training2Parameters = _configurationManager.GetTraining2Parameters();

            try
            {
                var learningFacade = new LearningFacade(DataProviderConfiguration, GlobalTrainerConfiguration, this);
                AvailableCategories = new ObservableCollection<Category>(learningFacade.GetAvailableCategories());

                ProgramState = ProgramState.ConfiguringTraining;
            }
            catch (Exception e)
            {
                LogWriteLine($"Problem with creating new training configuration. Error: {e.Message}");
            }
        }

        #region Load / save network

        private void LoadSavedNetwork()
        {
            var fileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Title = "Load network from selected save file",
                Multiselect = false,
                RestoreDirectory = true,
                DefaultExt = SavedNetworkFileExtension,
                AddExtension = true,
                Filter = $"Deep network file (*.{SavedNetworkFileExtension})|*.{SavedNetworkFileExtension}",
            };

            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            var fileName = fileDialog.FileName;
            var classifierConfiguration = new ClassifierConfiguration
            {
                Categories = SelectedCategories,
            };

            try
            {
                _classifierFacade = new ClassifierFacade(
                    savedNetworkPath:          fileName, 
                    dataProviderConfiguration: _dataProviderConfiguration, 
                    classifierConfiguration:   classifierConfiguration, 
                    logger:                    this);

                AvailableCategories = new ObservableCollection<Category>(_classifierFacade.GetAvailableCategories());

                LogWriteLine($"Successfully loaded saved network from file '{fileName}'.");

                ProgramState = ProgramState.ClassifierReady;
            }
            catch (Exception e)
            {
                LogWriteLine($"Problems with loading saved network. Error: {e.Message}");
            }
        }

        private void SaveNetwork()
        {
            var dateTimeFormatted = DateTime.Now.ToString("yyyyMMdd.HHmmss");

            var fileDialog = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Title = "Save trained network file",
                RestoreDirectory = true,
                AddExtension = true,
                Filter = $"Deep network file (*.{SavedNetworkFileExtension})|*.{SavedNetworkFileExtension}",
                DefaultExt = SavedNetworkFileExtension,
                FileName = $"Deep_{dateTimeFormatted}.{SavedNetworkFileExtension}",
            };

            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            var fileName = fileDialog.FileName;

            try
            {
                _classifierFacade.SaveClassifier(fileName);
                LogWriteLine($"Successfully saved network to the file '{fileName}'.");
            }
            catch (Exception e)
            {
                LogWriteLine($"Problems with saving network. Error: {e.Message}");
            }
        }

        #endregion

        private async void StartTraining()
        {
            if (SelectedCategories == null || SelectedCategories.Count < 2)
            {
                LogWriteLine("You cannot start a training until at least 2 categories are selected.");
                return;
            }

            ProgramState = ProgramState.TrainingInProgress;

            var learningFacade = new LearningFacade(DataProviderConfiguration, GlobalTrainerConfiguration,  this);
            var categories = SelectedCategories.Select((x, i) =>
            {
                x.Index = i;
                return x;
            });

            var trainingParameters = new TrainingParameters
            {
                Training1Parameters = Training1Parameters,
                Training2Parameters = Training2Parameters,
                SelectedCategories = categories,
            };

            try
            {
                _cts = new CancellationTokenSource();
                _classifierFacade = await learningFacade.RunTrainingForSelectedCategories(trainingParameters, _cts.Token);

                ProgramState = ProgramState.ClassifierReady;
            }
            catch (Exception e)
            {
                LogWriteLine($"Problems during training. Error: {e.Message}");
                ReturnToInitialWithSaving();
            }
        }

        private void ReturnToInitialWithSaving()
        {
            SaveConfiguration();

            AvailableCategories = null;
            SelectedCategories = null;
            GlobalTrainerConfiguration = null;
            Training1Parameters = null;
            Training2Parameters = null;

            ProgramState = ProgramState.Initial;
        }

        private void CancelComputing()
        {
            _cts.Cancel();
        }

        private void ClassifyImage()
        {
            if (SelectedCategories == null || SelectedCategories.Count < 2)
            {
                LogWriteLine("You cannot classify an image until at least 2 categories are selected.");
                return;
            }

            var fileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Title = "Select file to classify",
                Multiselect = false,
                RestoreDirectory = true,
            };

            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            var fileName = fileDialog.FileName;

            try
            {
                var classification = _classifierFacade.ClassifyToCategory(fileName);
                LogWriteLine($"Classified to category '{classification.Category}' with {classification.Probability} probability.");
            }
            catch (Exception e)
            {
                LogWriteLine($"Problems with classifying image of path '{fileName}'. Error: {e.Message}");
            }
        }

        public void SaveConfiguration()
        {
            _configurationManager.SaveConfigs(
                DataProviderConfiguration,
                GlobalTrainerConfiguration,
                Training1Parameters,
                Training2Parameters);
        }

        public void LogWriteLine(string logMessage)
        {
            const string separatorPattern = "-";
            const int separatorPatternMultiplier = 10;

            var date = DateTime.Now.ToString("HH:mm:ss.fff");
            var separator = Enumerable.Repeat(separatorPattern, separatorPatternMultiplier).Aggregate((x, y) => x + y);

            Application.Current?.Dispatcher.Invoke(() =>
            {
                OutputTextBoxContent += $"{date}\n{logMessage}\n{separator}\n";
            });
        }
    }
}

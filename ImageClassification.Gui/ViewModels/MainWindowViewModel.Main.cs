using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using NLog;
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
    public partial class MainWindowViewModel : ViewModelBase, IGuiLogger
    {
        private const string SavedNetworkFileExtension = "dbn";

        private ClassifierFacade _classifierFacade;

        private readonly IConfigurationManager _configurationManager;
        private CancellationTokenSource _cts;

        private ILogger NLogLogger { get; } = LogManager.GetCurrentClassLogger();

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
                var learningFacade = new LearningFacade(DataProviderConfiguration, GlobalTrainerConfiguration, SkipPhaseRequest, this);
                AvailableCategories = new ObservableCollection<Category>(learningFacade.GetAvailableCategories());

                ProgramState = ProgramState.ConfiguringTraining;
            }
            catch (Exception e)
            {
                const string errorMessage = "Problem with creating new training configuration.";

                LogWriteLine($"{errorMessage} Error: {e.Message}");
                NLogLogger.Error(e, errorMessage);
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
                    logger:                    this);

                AvailableCategories = new ObservableCollection<Category>(_classifierFacade.GetAvailableCategories());

                LogWriteLine($"Successfully loaded saved network from file '{fileName}'.");

                ProgramState = ProgramState.ClassifierReady;
            }
            catch (Exception e)
            {
                const string errorMessage = "Problems with loading saved network.";

                LogWriteLine($"{errorMessage} Error: {e.Message}");
                NLogLogger.Error(e, errorMessage);
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
                const string errorMessage = "Problems with saving network.";

                LogWriteLine($"{errorMessage} Error: {e.Message}");
                NLogLogger.Error(e, errorMessage);
            }
        }

        #endregion

        private async void StartTraining()
        {
            if (IsCategoriesSelectionInvalid())
            {
                LogWriteLine("You cannot start a training until at least 2 categories are selected.");
                return;
            }

            ProgramState = ProgramState.TrainingInProgress;

            var learningFacade = new LearningFacade(DataProviderConfiguration, GlobalTrainerConfiguration, SkipPhaseRequest, this);
            var categories = GetSelectedCategories();

            var trainingParameters = new TrainingParameters
            {
                Training1Parameters = Training1Parameters,
                Training2Parameters = Training2Parameters,
                SelectedCategories = categories.ToList(),
            };

            try
            {
                _cts = new CancellationTokenSource();
                _classifierFacade = await learningFacade.RunTrainingForSelectedCategories(trainingParameters, _cts.Token);

                ProgramState = ProgramState.ClassifierReady;
            }
            catch (Exception e)
            {
                const string errorMessage = "Problems during training.";

                LogWriteLine($"{errorMessage} Error: {e.Message}");
                NLogLogger.Error(e, errorMessage);

                ReturnToInitialWithSaving();
            }
        }

        private bool IsCategoriesSelectionInvalid()
        {
            return SelectedCategories?.Count < 2;
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
            if (IsCategoriesSelectionInvalid())
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
                var classification = _classifierFacade.ClassifyToCategory(fileName, new ClassifierConfiguration
                {
                    Categories = GetSelectedCategories(),
                });
                LogWriteLine($"Classified to category '{classification.Category}' with {classification.Probability} probability.");
            }
            catch (Exception e)
            {
                string errorMessage = $"Problems with classifying image of path '{fileName}'.";

                LogWriteLine($"{errorMessage} Error: {e.Message}");
                NLogLogger.Error(e, errorMessage);
            }
        }

        private IEnumerable<Category> GetSelectedCategories()
        {
            return SelectedCategories.OrderBy(x => x.Name).Select((x, i) =>
            {
                x.Index = i;
                return x;
            }).ToList();
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

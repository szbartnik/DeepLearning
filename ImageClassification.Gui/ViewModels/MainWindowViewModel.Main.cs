using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Wkiro.ImageClassification.Core.Facades;
using Wkiro.ImageClassification.Core.Infrastructure.Logging;
using Wkiro.ImageClassification.Core.Models.Configurations;
using Wkiro.ImageClassification.Core.Models.Dto;
using Wkiro.ImageClassification.Gui.Configuration;
using Wkiro.ImageClassification.Gui.Infrastructure;
using Wkiro.ImageClassification.Gui.Views;
using Application = System.Windows.Application;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace Wkiro.ImageClassification.Gui.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase, ILogger
    {
        private LearningFacade _learningFacade;
        private ClassifierFacade _classifierFacade;

        private readonly IConfigurationManager _configurationManager;

        public MainWindowViewModel(bool isNotDesignMode)
        {
            _configurationManager = new HardcodedConfigurationManager();

            InitializeCommands();
            DataProviderConfiguration = _configurationManager.GetInitialDataProviderConfiguration();
        }

        private void ConfigureNewTraining()
        {
            TrainerConfiguration = _configurationManager.GetInitialTrainerConfiguration();
            Training1Parameters = _configurationManager.GetInitialTraining1Parameters();
            Training2Parameters = _configurationManager.GetInitialTraining2Parameters();

            var learningFacade = new LearningFacade(DataProviderConfiguration, this);
            AvailableCategories = new ObservableCollection<Category>(learningFacade.GetAvailableCategories());
        }

        private void LoadTrainingData()
        {
            
        }

        private async void StartTraining()
        {
            _learningFacade = new LearningFacade(DataProviderConfiguration, this);
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
                Layers = TrainerConfiguration.Layers,
            };

            var task = _learningFacade.RunTrainingForSelectedCategories(trainingParameters);
            _classifierFacade = await task;
        }

        private void ClassifyImage()
        {
            var fileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Title = "Select file to classify",
                Multiselect = false,
                RestoreDirectory = true,
            };
            var result = fileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var fileName = fileDialog.FileName;
                var classification = _classifierFacade.ClassifyToCategory(fileName);
                LogWriteLine($"Classified to category: {classification.Category} with {classification.Probability} probability.");
            }

        }

        public void LogWriteLine(string logMessage)
        {
            const string separatorPattern = "-";
            const int separatorPatternMultiplier = 10;

            var date = DateTime.Now.ToString("HH:mm:ss.fff");
            var separator = Enumerable.Repeat(separatorPattern, separatorPatternMultiplier).Aggregate((x, y) => x + y);

            Application.Current.Dispatcher.Invoke(() =>
            {
                OutputTextBoxContent += $"{date}\n{logMessage}\n{separator}\n";
            });
        }
    }
}

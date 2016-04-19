using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using Wkiro.ImageClassification.Core.Facades;
using Wkiro.ImageClassification.Core.Infrastructure.Logging;
using Wkiro.ImageClassification.Core.Models.Configurations;
using Wkiro.ImageClassification.Core.Models.Dto;
using Wkiro.ImageClassification.Gui.Infrastructure;

namespace Wkiro.ImageClassification.Gui.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase, ILogger
    {
        private LearningFacade _learningFacade;

        public MainWindowViewModel(bool isNotDesignMode)
        {
            InitializeCommands();
        }

        private void InitializeLearningFacade()
        {
            DataProviderConfiguration = new DataProviderConfiguration
            {
                CropWidth = 300,
                CropHeight = 200,
                ProcessingWidth = 30,
                ProcessingHeight = 20,
                TrainFilesLocationPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileExtensions = new[] {"jpg"},
                TrainDataRatio = 0.8,
            };

            _learningFacade = new LearningFacade(DataProviderConfiguration, this);
        }

        private void ConfigureNewTraining()
        {
            InitializeLearningFacade();
            AvailableCategories = new ObservableCollection<Category>(_learningFacade.GetAvailableCategories());
        }

        private void LoadTrainingData()
        {
            
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

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using Wkiro.ImageClassification.Core.Facades;
using Wkiro.ImageClassification.Core.Infrastructure.Logging;
using Wkiro.ImageClassification.Core.Models.Dto;
using Wkiro.ImageClassification.Gui.Configuration;
using Wkiro.ImageClassification.Gui.Infrastructure;

namespace Wkiro.ImageClassification.Gui.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase, ILogger
    {
        private LearningFacade _learningFacade;
        private readonly IConfigurationManager _configurationManager;

        public MainWindowViewModel(bool isNotDesignMode)
        {
            _configurationManager = new ConfigurationManager();

            InitializeCommands();
            DataProviderConfiguration = _configurationManager.GetInitialDataProviderConfiguration();
        }

        private void ConfigureNewTraining()
        {
            _learningFacade = new LearningFacade(DataProviderConfiguration, this);
            Training1Parameters = _configurationManager.GetInitialTraining1Parameters();
            Training2Parameters = _configurationManager.GetInitialTraining2Parameters();

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

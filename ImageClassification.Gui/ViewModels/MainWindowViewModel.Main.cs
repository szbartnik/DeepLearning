using System;
using System.Linq;
using System.Windows;
using Wkiro.ImageClassification.Core.Infrastructure.Logging;
using Wkiro.ImageClassification.Core.Models.Configurations;
using Wkiro.ImageClassification.Gui.Infrastructure;

namespace Wkiro.ImageClassification.Gui.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase, ILogger
    {
        public MainWindowViewModel(bool isNotDesignMode)
        {
            InitializeCommands();

            DataProviderConfiguration = new DataProviderConfiguration
            {
                CropWidth = 300,
                CropHeight = 200,
                ProcessingWidth = 30,
                ProcessingHeight = 20,
                TrainFilesLocationPath = @"C:\Users\Szymon\Desktop\101_ObjectCategories",
                FileExtensions = new[] {"jpg"},
                TrainDataRatio = 0.8,
            };
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

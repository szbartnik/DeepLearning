using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using Wkiro.ImageClassification.Core.Infrastructure.Logging;
using Wkiro.ImageClassification.Core.Models.Configurations;
using Wkiro.ImageClassification.Core.Models.Dto;
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

            AvailableCategories = new ObservableCollection<Category>
            {
                new Category(0, "Test cat 1", @"C:\FakePath1\", new FileInfo[] {}),
                new Category(0, "Test cat 2", @"C:\FakePath2\", new FileInfo[] {}),
                new Category(0, "Test cat 3", @"C:\FakePath3\", new FileInfo[] {}),
                new Category(0, "Test cat 4", @"C:\FakePath4\", new FileInfo[] {}),
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

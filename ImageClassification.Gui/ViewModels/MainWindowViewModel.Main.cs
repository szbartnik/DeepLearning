using System;
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
                TrainFilesLocationPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileExtensions = new[] {"jpg"},
                TrainDataRatio = 0.8,
            };
        }

        private void LoadTrainingData()
        {
            
        }

		public void LogWriteLine(string logMessage)
        {
            OutputTextBoxContent += $"{logMessage}\n";
        }
    }
}

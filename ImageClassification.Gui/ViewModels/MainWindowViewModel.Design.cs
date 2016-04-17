using Wkiro.ImageClassification.Core.Models.Configurations;

namespace Wkiro.ImageClassification.Gui.ViewModels
{
    public partial class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            DataProviderConfiguration = new DataProviderConfiguration
            {
                TrainFilesLocationPath = @"C:\Users\User\file.dat",
                CropHeight = 123,
                CropWidth = 456,
                ProcessingHeight = 789,
                ProcessingWidth = 987,
                FileExtensions = new[] { "JPG", "BMP" },
                TrainDataRatio = 0.8,
            };
        }
    }
}

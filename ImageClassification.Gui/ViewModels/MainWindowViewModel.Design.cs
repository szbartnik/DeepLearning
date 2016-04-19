using System.Collections.ObjectModel;
using System.IO;
using Wkiro.ImageClassification.Core.Models.Configurations;
using Wkiro.ImageClassification.Core.Models.Dto;

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
                CropWidth = 453,
                ProcessingHeight = 789,
                ProcessingWidth = 987,
                FileExtensions = new[] { "JPG", "BMP" },
                TrainDataRatio = 0.8,
            };

            Training1Parameters = new Training1Parameters
            {
                Momentum = 0.5,
                Decay = 0.001,
                LearningRate = 0.1,
                UnsupervisedEpochs = 200,
            };

            Training2Parameters = new Training2Parameters
            {
                Momentum = 0.5,
                LearningRate = 0.1,
                SupervisedEpochs = 300,
            };

            AvailableCategories = new ObservableCollection<Category>
            {
                new Category(0, "Test cat 1", @"C:\FakePath1\", new FileInfo[] {}),
                new Category(0, "Test cat 2", @"C:\FakePath2\", new FileInfo[] {}),
                new Category(0, "Test cat 3", @"C:\FakePath3\", new FileInfo[] {}),
                new Category(0, "Test cat 4", @"C:\FakePath4\", new FileInfo[] {}),
            };

            SelectedCategories = new ObservableCollection<Category>(new []{ AvailableCategories[1], AvailableCategories[2]});
        }

    }
}

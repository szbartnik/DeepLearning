using System;
using System.Collections.ObjectModel;
using System.IO;
using Wkiro.ImageClassification.Core.Models.Configurations;
using Wkiro.ImageClassification.Core.Models.Dto;
using Wkiro.ImageClassification.Gui.Infrastructure;

namespace Wkiro.ImageClassification.Gui.ViewModels
{
    public partial class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            DataProviderConfiguration = new DataProviderConfiguration
            {
                TrainFilesLocationPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                CropHeight = 123,
                CropWidth = 456,
                ProcessingHeight = 789,
                ProcessingWidth = 987,
                FileExtensions = new[] { "JPG", "BMP" },
                TrainDataRatio = 0.8,
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

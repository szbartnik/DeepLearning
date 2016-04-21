using System.Collections.ObjectModel;
using System.IO;
using Wkiro.ImageClassification.Core.Models.Dto;
using Wkiro.ImageClassification.Gui.Configuration;
using Wkiro.ImageClassification.Gui.Models;

namespace Wkiro.ImageClassification.Gui.ViewModels
{
    public partial class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            var configurationManager = new HardcodedConfigurationManager();

            ProgramState = ProgramState.ClassifierReady;

            DataProviderConfiguration = configurationManager.GetDataProviderConfiguration();
            GlobalTrainerConfiguration = configurationManager.GetGlobalTrainerConfiguration();
            Training1Parameters = configurationManager.GetTraining1Parameters();
            Training2Parameters = configurationManager.GetTraining2Parameters();

            AvailableCategories = new ObservableCollection<Category>
            {
                new Category("Test cat 1", @"C:\FakePath1\", new FileInfo[] {}),
                new Category("Test cat 2", @"C:\FakePath2\", new FileInfo[] {}),
                new Category("Test cat 3", @"C:\FakePath3\", new FileInfo[] {}),
                new Category("Test cat 4", @"C:\FakePath4\", new FileInfo[] {}),
            };

            SelectedCategories = new ObservableCollection<Category>(new []{ AvailableCategories[1], AvailableCategories[2]});
        }
    }
}

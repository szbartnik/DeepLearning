using Wkiro.ImageClassification.Core.Models.Configurations;
using Wkiro.ImageClassification.Gui.Infrastructure;

namespace Wkiro.ImageClassification.Gui.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            DataProviderConfiguration = new DataProviderConfiguration
            {
                FilesLocationPath = @"C:\Users\User\file.dat",
            };
        }
    }
}

using Wkiro.ImageClassification.Infrastructure.Wpf;
using Wkiro.ImageClassification.Models.Configurations;

namespace Wkiro.ImageClassification.ViewModels
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

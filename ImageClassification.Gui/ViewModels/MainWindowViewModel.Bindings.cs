using Wkiro.ImageClassification.Infrastructure.Wpf;
using Wkiro.ImageClassification.Models.Configurations;

namespace Wkiro.ImageClassification.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public DataProviderConfiguration DataProviderConfiguration
        {
            get { return _dataProviderConfiguration; }
            set
            {
                _dataProviderConfiguration = value;
                OnPropertyChanged();
            }
        }
        private DataProviderConfiguration _dataProviderConfiguration;
    }
}

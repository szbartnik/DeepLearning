using Polsl.Inf.Os2.WKiRO.ImageClassification.Infrastructure.Wpf;
using Polsl.Inf.Os2.WKiRO.ImageClassification.Models.Configurations;

namespace Polsl.Inf.Os2.WKiRO.ImageClassification.ViewModels
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

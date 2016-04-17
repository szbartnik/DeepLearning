using Wkiro.ImageClassification.Core.Models.Configurations;
using Wkiro.ImageClassification.Gui.Infrastructure;

namespace Wkiro.ImageClassification.Gui.ViewModels
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

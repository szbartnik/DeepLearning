using Wkiro.ImageClassification.Core.Models.Configurations;

namespace Wkiro.ImageClassification.Gui.ViewModels
{
    public partial class MainWindowViewModel
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

        public string OutputTextBoxContent
        {
            get { return _outputTextBoxContent; }
            set
            {
                _outputTextBoxContent = value;
                OnPropertyChanged();
            }
        }
        private string _outputTextBoxContent;
    }
}

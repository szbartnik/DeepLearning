using System.Collections.ObjectModel;
using Wkiro.ImageClassification.Core.Models.Configurations;
using Wkiro.ImageClassification.Core.Models.Dto;

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

        public ObservableCollection<Category> AvailableCategories
        {
            get { return _availableCategories; }
            set
            {
                _availableCategories = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Category> _availableCategories;

        public ObservableCollection<Category> SelectedCategories
        {
            get { return _selectedCategories; }
            set
            {
                _selectedCategories = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Category> _selectedCategories;
    }
}

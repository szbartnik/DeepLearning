using System.Collections.ObjectModel;
using Wkiro.ImageClassification.Core.Models.Configurations;
using Wkiro.ImageClassification.Core.Models.Dto;
using Wkiro.ImageClassification.Gui.Models;

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

        public GlobalTrainerConfiguration GlobalTrainerConfiguration
        {
            get { return _globalTrainerConfiguration; }
            set
            {
                _globalTrainerConfiguration = value;
                OnPropertyChanged();
            }
        }
        private GlobalTrainerConfiguration _globalTrainerConfiguration;

        public Training1Parameters Training1Parameters
        {
            get { return _training1Parameters; }
            set
            {
                _training1Parameters = value;
                OnPropertyChanged();
            }
        }
        private Training1Parameters _training1Parameters;

        public Training2Parameters Training2Parameters
        {
            get { return _training2Parameters; }
            set
            {
                _training2Parameters = value;
                OnPropertyChanged();
            }
        }
        private Training2Parameters _training2Parameters;

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

        public ProgramState ProgramState
        {
            get { return _programState; }
            set
            {
                _programState = value; 
                OnPropertyChanged();
            }
        }
        private ProgramState _programState;

    }
}

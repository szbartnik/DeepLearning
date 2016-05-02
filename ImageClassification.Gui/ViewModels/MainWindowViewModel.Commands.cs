using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Wkiro.ImageClassification.Core.Models.Dto;
using Wkiro.ImageClassification.Gui.Infrastructure;

namespace Wkiro.ImageClassification.Gui.ViewModels
{
    public partial class MainWindowViewModel
    {
        public RelayCommand BrowseForTrainFilesPathCommand { get; set; }
        public RelayCommand ConfigureNewTrainingCommand { get; set; }
        public RelayCommand LoadSavedNetworkCommand { get; set; }
        public RelayCommand<object> SelectedCategoriesChangedCommand { get; set; }
        public RelayCommand StartTrainingCommand { get; set; }
        public RelayCommand ClassifyImageCommand { get; set; }
        public RelayCommand SaveNetworkCommand { get; set; }
        public RelayCommand ReconfigureCommand { get; set; }
        public RelayCommand CancelComputingCommand { get; set; }

        private void InitializeCommands()
        {
            BrowseForTrainFilesPathCommand = new RelayCommand(BrowseForTrainFilesPath);
            ConfigureNewTrainingCommand = new RelayCommand(ConfigureNewTraining);
            LoadSavedNetworkCommand = new RelayCommand(LoadSavedNetwork);
            SelectedCategoriesChangedCommand = new RelayCommand<object>(SelectedCategoriesChanged);

            StartTrainingCommand = new RelayCommand(StartTraining);
            ClassifyImageCommand = new RelayCommand(ClassifyImage);
            CancelComputingCommand = new RelayCommand(CancelComputing);

            SaveNetworkCommand = new RelayCommand(SaveNetwork);
            ReconfigureCommand = new RelayCommand(Reconfigure);
        }

        private void Reconfigure()
        {
            ReturnToInitialWithSaving();
        }

        private void BrowseForTrainFilesPath()
        {
            var directory = _dataProviderConfiguration.TrainFilesLocationPath;
            var dialog = new FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.MyComputer,
                Description = "Select directory containing training folders",
                ShowNewFolderButton = false
            };

            if (Directory.Exists(directory))
                dialog.SelectedPath = directory;

            if (dialog.ShowDialog() == DialogResult.OK)
                _dataProviderConfiguration.TrainFilesLocationPath = dialog.SelectedPath;
        }

        private void SelectedCategoriesChanged(object categories)
        {
            var casted = ((IList) categories).Cast<Category>();
            SelectedCategories = new ObservableCollection<Category>(casted);
        }
    }
}

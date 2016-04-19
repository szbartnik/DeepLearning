using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using Wkiro.ImageClassification.Gui.Infrastructure;
using Wkiro.ImageClassification.Core.Models.Configurations;

namespace Wkiro.ImageClassification.Gui.ViewModels
{
    public partial class MainWindowViewModel
    {
        public RelayCommand BrowseForTrainFilesPathCommand { get; set; }
        public RelayCommand LoadTrainingDataCommand { get; set; }
		public RelayCommand UseMNISTDatasetDataCommand { get; set; }

		private void InitializeCommands()
        {
            BrowseForTrainFilesPathCommand = new RelayCommand(BrowseForTrainFilesPath);
            LoadTrainingDataCommand = new RelayCommand(LoadTrainingData);
			UseMNISTDatasetDataCommand = new RelayCommand(UseMNISTDataset);

		}

        private void BrowseForTrainFilesPath()
        {
            var directory = _dataProviderConfiguration.TrainFilesLocationPath;
            var dialog = new FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.DesktopDirectory,
                Description = "Select directory containing training folders",
                ShowNewFolderButton = false
            };

            if (Directory.Exists(directory))
                dialog.SelectedPath = directory;

            if (dialog.ShowDialog() == DialogResult.OK)
                _dataProviderConfiguration.TrainFilesLocationPath = dialog.SelectedPath;
        }

		private void UseMNISTDataset()
		{
			var directory = DataProviderConfiguration.TrainFilesLocationPath;
			DataProviderConfiguration = new DataProviderConfiguration
			{
				CropHeight = 28,
				CropWidth = 28,
				ProcessingHeight = 28,
				ProcessingWidth = 28,
				TrainDataRatio = 1.0,
				TrainFilesLocationPath = directory,
				FileExtensions = new string[] { "idx3-ubyte", "idx1-ubyte" }
			};
			
			var dialog = new FolderBrowserDialog
			{
				RootFolder = Environment.SpecialFolder.MyComputer,
				Description = "Select directory containing MNIST dataset",
				ShowNewFolderButton = false
			};

			if (Directory.Exists(directory))
				dialog.SelectedPath = directory;

			if (dialog.ShowDialog() == DialogResult.OK)
				DataProviderConfiguration.TrainFilesLocationPath = dialog.SelectedPath;

			DataProviderConfiguration.CropHeight = 28;

		}
	}
}

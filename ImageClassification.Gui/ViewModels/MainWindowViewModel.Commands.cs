using Wkiro.ImageClassification.Core.Models.Configurations;
using Wkiro.ImageClassification.Gui.Infrastructure;

namespace Wkiro.ImageClassification.Gui.ViewModels
{
    public partial class MainWindowViewModel
    {
        public RelayCommand BrowseForTrainFilesPath { get; set; }

        private void InitializeCommands()
        {
            BrowseForTrainFilesPath = new RelayCommand(() =>
            {
                
            });
        }
    }
}

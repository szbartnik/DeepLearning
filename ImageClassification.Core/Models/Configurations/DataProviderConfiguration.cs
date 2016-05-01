namespace Wkiro.ImageClassification.Core.Models.Configurations
{
    public class DataProviderConfiguration : PropertyChangedNotifier
    {
        private string _trainFilesLocationPath;
        private string[] _fileExtensions;
        
        private int _processingWidth;
        private int _processingHeight;

        public string TrainFilesLocationPath
        {
            get { return _trainFilesLocationPath; }
            set
            {
                _trainFilesLocationPath = value;
                OnPropertyChanged();
            }
        }
        public string[] FileExtensions
        {
            get { return _fileExtensions; }
            set
            {
                _fileExtensions = value;
                OnPropertyChanged();
            }
        }
        
        public int ProcessingWidth
        {
            get { return _processingWidth; }
            set
            {
                _processingWidth = value;
                OnPropertyChanged();
            }
        }
        public int ProcessingHeight
        {
            get { return _processingHeight; }
            set
            {
                _processingHeight = value;
                OnPropertyChanged();
            }
        }

    }
}

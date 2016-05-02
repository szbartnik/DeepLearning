namespace Wkiro.ImageClassification.Core.Models.Configurations
{
    public class DataProviderConfiguration : PropertyChangedNotifier
    {
        private string _trainFilesLocationPath;
        private string[] _fileExtensions;
        
        private int _processingWidth;
        private int _processingHeight;
        private bool _useGrayScale;
        private bool _shouldAutoCrop;
        private bool _shouldEqualizeHistogram;

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

        public bool UseGrayScale
        {
            get { return _useGrayScale; }
            set
            {
                _useGrayScale = value;
                OnPropertyChanged();
            }
        }

        public bool ShouldAutoCrop
        {
            get { return _shouldAutoCrop; }
            set
            {
                _shouldAutoCrop = value;
                OnPropertyChanged();
            }
        }

        public bool ShouldEqualizeHistorgram
        {
            get { return _shouldEqualizeHistogram; }
            set
            {
                _shouldEqualizeHistogram = value;
                OnPropertyChanged();
            }
        }

    }
}

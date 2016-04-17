using System.ComponentModel;
using System.Runtime.CompilerServices;
using Wkiro.ImageClassification.Core.Annotations;

namespace Wkiro.ImageClassification.Core.Models.Configurations
{
    public class DataProviderConfiguration : INotifyPropertyChanged
    {
        private string _trainFilesLocationPath;
        private string[] _fileExtensions;
        private double _trainDataRatio;
        private int _processingWidth;
        private int _processingHeight;
        private int _cropWidth;
        private int _cropHeight;

        public string TrainFilesLocationPath
        {
            get { return _trainFilesLocationPath; }
            set
            {
                if (value == _trainFilesLocationPath) return;
                _trainFilesLocationPath = value;
                OnPropertyChanged();
            }
        }
        public string[] FileExtensions
        {
            get { return _fileExtensions; }
            set
            {
                if (Equals(value, _fileExtensions)) return;
                _fileExtensions = value;
                OnPropertyChanged();
            }
        }
        public double TrainDataRatio
        {
            get { return _trainDataRatio; }
            set
            {
                if (value.Equals(_trainDataRatio)) return;
                _trainDataRatio = value;
                OnPropertyChanged();
            }
        }
        public int ProcessingWidth
        {
            get { return _processingWidth; }
            set
            {
                if (value == _processingWidth) return;
                _processingWidth = value;
                OnPropertyChanged();
            }
        }
        public int ProcessingHeight
        {
            get { return _processingHeight; }
            set
            {
                if (value == _processingHeight) return;
                _processingHeight = value;
                OnPropertyChanged();
            }
        }
        public int CropWidth
        {
            get { return _cropWidth; }
            set
            {
                if (value == _cropWidth) return;
                _cropWidth = value;
                OnPropertyChanged();
            }
        }
        public int CropHeight
        {
            get { return _cropHeight; }
            set
            {
                if (value == _cropHeight) return;
                _cropHeight = value;
                OnPropertyChanged();
            }
        }

        #region Property changed stuff

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}

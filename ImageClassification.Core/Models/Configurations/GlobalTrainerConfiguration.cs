namespace Wkiro.ImageClassification.Core.Models.Configurations
{
    public class GlobalTrainerConfiguration : PropertyChangedNotifier
    {
        public int[] HiddenLayers
        {
            get { return _hiddenLayers; }
            set
            {
                if (Equals(value, _hiddenLayers)) return;
                _hiddenLayers = value;
                OnPropertyChanged();
            }
        }
        private int[] _hiddenLayers;

        public double TrainDataRatio
        {
            get { return _trainDataRatio; }
            set
            {
                _trainDataRatio = value;
                OnPropertyChanged();
            }
        }
        private double _trainDataRatio;

    }
}
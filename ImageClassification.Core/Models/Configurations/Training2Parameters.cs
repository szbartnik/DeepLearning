namespace Wkiro.ImageClassification.Core.Models.Configurations
{
    public class Training2Parameters : PropertyChangedNotifier
    {
        private double _learningRate;
        private double _momentum;
        private int _supervisedEpochs;

        public double LearningRate
        {
            get { return _learningRate; }
            set
            {
                _learningRate = value;
                OnPropertyChanged();
            }
        }
        public double Momentum
        {
            get { return _momentum; }
            set
            {
                _momentum = value;
                OnPropertyChanged();
            }
        }
        public int SupervisedEpochs
        {
            get { return _supervisedEpochs; }
            set
            {
                _supervisedEpochs = value;
                OnPropertyChanged();
            }
        }

    }
}
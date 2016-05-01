namespace Wkiro.ImageClassification.Core.Models.Configurations
{
    public class Training1Parameters : PropertyChangedNotifier
    {
        private double _learningRate;
        private double _momentum;
        private int _unsupervisedEpochs;
        private double _decay;

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
        public int UnsupervisedEpochs
        {
            get { return _unsupervisedEpochs; }
            set
            {
                _unsupervisedEpochs = value;
                OnPropertyChanged();
            }
        }
        public double Decay
        {
            get { return _decay; }
            set
            {
                _decay = value;
                OnPropertyChanged();
            }
        }

    }
}
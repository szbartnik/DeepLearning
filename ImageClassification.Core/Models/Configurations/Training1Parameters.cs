using System.ComponentModel;
using System.Runtime.CompilerServices;
using Wkiro.ImageClassification.Core.Annotations;

namespace Wkiro.ImageClassification.Core.Models.Configurations
{
    public class Training1Parameters : INotifyPropertyChanged
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
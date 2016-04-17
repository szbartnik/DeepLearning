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
                if (value.Equals(_learningRate)) return;
                _learningRate = value;
                OnPropertyChanged();
            }
        }
        public double Momentum
        {
            get { return _momentum; }
            set
            {
                if (value.Equals(_momentum)) return;
                _momentum = value;
                OnPropertyChanged();
            }
        }
        public int UnsupervisedEpochs
        {
            get { return _unsupervisedEpochs; }
            set
            {
                if (value == _unsupervisedEpochs) return;
                _unsupervisedEpochs = value;
                OnPropertyChanged();
            }
        }
        public double Decay
        {
            get { return _decay; }
            set
            {
                if (value.Equals(_decay)) return;
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
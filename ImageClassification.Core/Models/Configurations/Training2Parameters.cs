using System.ComponentModel;
using System.Runtime.CompilerServices;
using Wkiro.ImageClassification.Core.Annotations;

namespace Wkiro.ImageClassification.Core.Models.Configurations
{
    public class Training2Parameters : INotifyPropertyChanged
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
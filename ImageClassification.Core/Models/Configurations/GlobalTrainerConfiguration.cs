using System.ComponentModel;
using System.Runtime.CompilerServices;
using Wkiro.ImageClassification.Core.Annotations;

namespace Wkiro.ImageClassification.Core.Models.Configurations
{
    public class GlobalTrainerConfiguration : INotifyPropertyChanged
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
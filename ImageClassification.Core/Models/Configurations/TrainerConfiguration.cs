using System.ComponentModel;
using System.Runtime.CompilerServices;
using Wkiro.ImageClassification.Core.Annotations;
using Wkiro.ImageClassification.Core.Models.Dto;

namespace Wkiro.ImageClassification.Core.Models.Configurations
{
    public class TrainerConfiguration : INotifyPropertyChanged
    {
        public InputOutputsDataNative InputsOutputsData { get; set; }

        public int[] Layers
        {
            get { return _layers; }
            set
            {
                _layers = value; 
                OnPropertyChanged();
            }
        }
        private int[] _layers;

        #region Property Changed stuff

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
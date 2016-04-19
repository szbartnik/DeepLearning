using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Wkiro.ImageClassification.Core.Annotations;
using Wkiro.ImageClassification.Core.Models.Dto;

namespace Wkiro.ImageClassification.Core.Models.Configurations
{
    public class TrainingParameters : INotifyPropertyChanged
    {
        public int[] Layers
        {
            get { return _layers; }
            set
            {
                if (Equals(value, _layers)) return;
                _layers = value;
                OnPropertyChanged();
            }
        }
        private int[] _layers;

        public Training1Parameters Training1Parameters { get; set; }
        public Training2Parameters Training2Parameters { get; set; }
        public ObservableCollection<Category> SelectedCategories { get; set; }

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

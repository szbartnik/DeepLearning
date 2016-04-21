using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Wkiro.ImageClassification.Core.Annotations;
using Wkiro.ImageClassification.Core.Models.Dto;

namespace Wkiro.ImageClassification.Core.Models.Configurations
{
    public class TrainingParameters : INotifyPropertyChanged
    {
        public Training1Parameters Training1Parameters { get; set; }
        public Training2Parameters Training2Parameters { get; set; }
        public IEnumerable<Category> SelectedCategories { get; set; }

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

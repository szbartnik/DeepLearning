using System.Collections.Generic;
using Wkiro.ImageClassification.Core.Models.Dto;

namespace Wkiro.ImageClassification.Core.Models.Configurations
{
    public class TrainingParameters : PropertyChangedNotifier
    {
        public Training1Parameters Training1Parameters { get; set; }
        public Training2Parameters Training2Parameters { get; set; }
        public IEnumerable<Category> SelectedCategories { get; set; }
    }
}

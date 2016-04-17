using System.Collections.Generic;
using Wkiro.ImageClassification.Models.Dto;

namespace Wkiro.ImageClassification.Models.Configurations
{
    public class TrainingParameters
    {
        public int[] Layers { get; set; }
        public Training1Parameters Training1Parameters { get; set; }
        public Training2Parameters Training2Parameters { get; set; }
        public List<Category> SelectedCategories { get; set; }
    }
}

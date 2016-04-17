using System.Collections.Generic;
using Wkiro.ImageClassification.Models.Dto;

namespace Wkiro.ImageClassification.Models.Configurations
{
    public class ClassifierConfiguration
    {
        public IEnumerable<Category> Categories { get; set; }
    }
}

using System.Collections.Generic;
using Wkiro.ImageClassification.Core.Models.Dto;

namespace Wkiro.ImageClassification.Core.Models.Configurations
{
    public class ClassifierConfiguration
    {
        public IEnumerable<Category> Categories { get; set; }
    }
}

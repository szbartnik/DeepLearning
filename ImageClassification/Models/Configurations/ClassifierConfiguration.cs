using System.Collections.Generic;
using ImageClassification.Models.Dto;

namespace ImageClassification.Models.Configurations
{
    public class ClassifierConfiguration
    {
        public IEnumerable<Category> Categories { get; set; }
    }
}

using System.Collections.Generic;
using Polsl.Inf.Os2.WKiRO.ImageClassification.Models.Dto;

namespace Polsl.Inf.Os2.WKiRO.ImageClassification.Models.Configurations
{
    public class ClassifierConfiguration
    {
        public IEnumerable<Category> Categories { get; set; }
    }
}

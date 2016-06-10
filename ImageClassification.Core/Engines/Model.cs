using Accord.Neuro.Networks;
using Wkiro.ImageClassification.Core.Models.Configurations;

namespace Wkiro.ImageClassification.Core.Engines
{
    public class Model
    {
        public DeepBeliefNetwork Network { get; set; }
        public ClassifierConfiguration ClassifierConfiguration { get; set; }
        public DataProviderConfiguration DataProviderConfiguration { get; set; }
    }
}

using System.Linq;
using Accord.Math;
using Accord.Neuro.Networks;
using Wkiro.ImageClassification.Core.Infrastructure.Logging;
using Wkiro.ImageClassification.Core.Models.Configurations;
using Wkiro.ImageClassification.Core.Models.Dto;

namespace Wkiro.ImageClassification.Core.Engines
{
    internal class Classifier
    {
        private readonly IGuiLogger _logger;
        private readonly DeepBeliefNetwork _network;
        private readonly ClassifierConfiguration _configuration;

        public DeepBeliefNetwork Network => _network;
        public ClassifierConfiguration ClassifierConfiguration => _configuration;

        private Classifier(IGuiLogger logger, ClassifierConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public Classifier(
            DeepBeliefNetwork network,
            ClassifierConfiguration configuration,
            IGuiLogger logger) : this(logger, configuration)
        {
            _network = network;
        }

        public CategoryClassification ClassifyToCategory(double[] dataToClassify)
        {
            var categories = _configuration.Categories;
            var output = _network.Compute(dataToClassify);
            var categoryIndex = GetIndexOfResult(output);
            var predictedCategory = categories.Single(x => x.Index == categoryIndex);

            _logger.LogWriteLine($"Prediction: {predictedCategory}");

            var result = new CategoryClassification(
                predictedCategory,
                output.Max());

            return result;
        }

        private static int GetIndexOfResult(double[] output) => output.IndexOf(output.Max());

    }
}

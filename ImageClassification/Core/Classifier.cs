using System.Linq;
using Accord.Math;
using Accord.Neuro.Networks;
using ImageClassification.Infrastructure.Logging;
using ImageClassification.Models.Configurations;
using ImageClassification.Models.Dto;

namespace ImageClassification.Core
{
    public class Classifier
    {
        private readonly ClassifierConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly DeepBeliefNetwork _network;

        private Classifier(ClassifierConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public Classifier(
            DeepBeliefNetwork network, 
            ClassifierConfiguration configuration, 
            ILogger logger) : this(configuration, logger)
        {
            _network = network;
        }

        public Classifier(
            string savedNetworkFilePath, 
            ClassifierConfiguration configuration, 
            ILogger logger) : this(configuration, logger)
        {
            _network = DeepBeliefNetwork.Load(savedNetworkFilePath);
        }

        public CategoryClassification ClassifyToCategory(double[] dataToClassify)
        {
            var categories = _configuration.Categories;

            var output = _network.Compute(dataToClassify);
            var categoryIndex = GetIndexOfResult(output);
            var predictedCategory = categories.Single(x => x.Index == categoryIndex);

            _logger.WriteLine($"Prediction: {predictedCategory}");

            var result = new CategoryClassification(
                predictedCategory,
                output.Max());

            return result;
        }

        private static int GetIndexOfResult(double[] output)
        {
            return output.ToList().IndexOf(output.Max());
        }
    }
}

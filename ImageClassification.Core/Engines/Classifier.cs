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

        private Classifier(IGuiLogger logger)
        {
            _logger = logger;
        }

        public Classifier(
            DeepBeliefNetwork network, 
            IGuiLogger logger) : this(logger)
        {
            _network = network;
        }

        public Classifier(
            string savedNetworkFilePath, 
            IGuiLogger logger) : this(logger)
        {
            _network = DeepBeliefNetwork.Load(savedNetworkFilePath);
        }

        public CategoryClassification ClassifyToCategory(double[] dataToClassify, ClassifierConfiguration configuration)
        {
            var categories = configuration.Categories;
            var output = _network.Compute(dataToClassify);
            var categoryIndex = GetIndexOfResult(output);
            var predictedCategory = categories.Single(x => x.Index == categoryIndex);

            _logger.LogWriteLine($"Prediction: {predictedCategory}");

            var result = new CategoryClassification(
                predictedCategory,
                output.Max());

            return result;
        }

        public void SaveClassifier(string saveLocationFilePath)
        {
            _network.Save(saveLocationFilePath);
        }

        private static int GetIndexOfResult(double[] output)
        {
            return output.ToList().IndexOf(output.Max());
        }
    }
}

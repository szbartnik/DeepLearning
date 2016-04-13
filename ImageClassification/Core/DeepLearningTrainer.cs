using System;
using System.Linq;
using Accord.Math;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Neuro.Networks;
using AForge.Neuro.Learning;
using ImageClassification.Infrastructure;
using ImageClassification.Models;

namespace ImageClassification.Core
{
    public class DeepLearningTrainer
    {
        private readonly TrainerConfiguration _configuration;
        private readonly DeepBeliefNetwork _networkToTrain;

        private readonly ILogger _logger;

        public DeepLearningTrainer(TrainerConfiguration configuration, ILogger logger)
        {
            _networkToTrain = CreateNetworkToTeach(configuration);
            _configuration  = configuration;
            _logger         = logger;
        }

        private static DeepBeliefNetwork CreateNetworkToTeach(TrainerConfiguration configuration)
        {
            var inputs = configuration.TrainingData.Inputs;

            // Setup the deep belief network and initialize with random weights.
            var network = new DeepBeliefNetwork(inputs.First().Length, configuration.Layers);
            new GaussianWeights(network).Randomize();
            network.UpdateVisibleWeights();

            return network;
        }

        public void RunTraining1(Training1Parameters parameters)
        {
            var teacher = new DeepBeliefNetworkLearning(_networkToTrain)
            {
                Algorithm = (hiddenLayer, visibleLayer, i) => new ContrastiveDivergenceLearning(hiddenLayer, visibleLayer)
                {
                    LearningRate = parameters.LearningRate,
                    Momentum     = parameters.Momentum,
                    Decay        = parameters.Decay,
                }
            };

            var inputs = _configuration.TrainingData.Inputs;

            // Setup batches of input for learning.
            var batchCount = Math.Max(1, inputs.Length / 100);

            // Create mini-batches to speed learning.
            var groups = Accord.Statistics.Tools.RandomGroups(inputs.Length, batchCount);
            var batches = inputs.Subgroups(groups);

            // Unsupervised learning on each hidden layer, except for the output layer.
            for (int layerIndex = 0; layerIndex < _networkToTrain.Machines.Count - 1; layerIndex++)
            {
                teacher.LayerIndex = layerIndex;
                var layerData = teacher.GetLayerInput(batches);
                for (int i = 0; i < parameters.SupervisedEpochs; i++)
                {
                    var error = teacher.RunEpoch(layerData) / inputs.Length;
                    if (i %10 != 0)
                        continue;

                    _logger.WriteLine($"Layer: {layerIndex} Epoch: {i}, Error: {error}");
                }
            }
        }

        public void RunTraining2(Training2Parameters parameters)
        {
            var trainingData = _configuration.TrainingData;

            var teacher = new BackPropagationLearning(_networkToTrain)
            {
                LearningRate = parameters.LearningRate,
                Momentum     = parameters.Momentum,
            };

            for (int i = 0; i < parameters.SupervisedEpochs; i++)
            {
                var error = teacher.RunEpoch(trainingData.Inputs, trainingData.Outputs) / trainingData.Inputs.Length;
                if (i % 10 != 0)
                    continue;

                _logger.WriteLine($"Supervised: {i}, Error = {error}");
            }
        }

        public void CheckAccuracy(TrainingData testData)
        {
            var correctnessFactor = 0;

            for (int i = 0; i < testData.Inputs.Length; i++)
            {
                var outputValues = _networkToTrain.Compute(testData.Inputs[i]);

                var predicted = GetResult(outputValues);
                var actual = GetResult(testData.Outputs[i]);

                if (predicted == actual)
                    correctnessFactor++;

                _logger.WriteLine($"predicted: {predicted} actual: {actual}");
            }

            _logger.WriteLine($"Correct {Math.Round(correctnessFactor / (double)testData.Inputs.Length * 100, 2)}%");
        }

        public Category ClassifyToCategory(double[] dataToClassify)
        {
            var categories = _configuration.Categories;

            var output = _networkToTrain.Compute(dataToClassify);
            var categoryIndex = GetResult(output);
            var predictedCategory = categories.Single(x => x.Index == categoryIndex);

            _logger.WriteLine($"Prediction: {predictedCategory}");

            return predictedCategory;
        }

        private static int GetResult(double[] output)
        {
            return output.ToList().IndexOf(output.Max());
        }
    }
}

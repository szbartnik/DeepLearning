using System;
using System.Linq;
using Accord.Math;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Neuro.Networks;
using AForge.Neuro.Learning;
using Wkiro.ImageClassification.Core.Infrastructure.Logging;
using Wkiro.ImageClassification.Core.Models.Configurations;
using Wkiro.ImageClassification.Core.Models.Dto;

namespace Wkiro.ImageClassification.Core.Engines
{
    internal class Trainer
    {
        private readonly TrainerConfiguration _configuration;

        public DeepBeliefNetwork NeuralNetwork => _neuralNetwork;
        private readonly DeepBeliefNetwork _neuralNetwork;

        private readonly IGuiLogger _logger;

        public Trainer(TrainerConfiguration configuration, IGuiLogger logger)
        {
            _neuralNetwork = CreateNetworkToTeach(configuration);
            _configuration  = configuration;
            _logger         = logger;
        }

        private static DeepBeliefNetwork CreateNetworkToTeach(TrainerConfiguration configuration)
        {
            var inputs = configuration.InputsOutputsData.Inputs;

            // Setup the deep belief network and initialize with random weights.
            var network = new DeepBeliefNetwork(inputs.First().Length, configuration.Layers);
            new GaussianWeights(network).Randomize();
            network.UpdateVisibleWeights();

            return network;
        }

        public void RunTraining1(Training1Parameters parameters)
        {
            var teacher = new DeepBeliefNetworkLearning(_neuralNetwork)
            {
                Algorithm = (hiddenLayer, visibleLayer, i) => new ContrastiveDivergenceLearning(hiddenLayer, visibleLayer)
                {
                    LearningRate = parameters.LearningRate,
                    Momentum     = parameters.Momentum,
                    Decay        = parameters.Decay,
                }
            };

            var inputs = _configuration.InputsOutputsData.Inputs;

            // Setup batches of input for learning.
            var batchCount = Math.Max(1, inputs.Length / 100);

            // Create mini-batches to speed learning.
            var groups = Accord.Statistics.Tools.RandomGroups(inputs.Length, batchCount);
            var batches = inputs.Subgroups(groups);

            // Unsupervised learning on each hidden layer, except for the output layer.
            for (int layerIndex = 0; layerIndex < _neuralNetwork.Machines.Count - 1; layerIndex++)
            {
                teacher.LayerIndex = layerIndex;
                var layerData = teacher.GetLayerInput(batches);
                for (int i = 0; i < parameters.UnsupervisedEpochs; i++)
                {
                    var error = teacher.RunEpoch(layerData) / inputs.Length;
                    if (i %10 != 0)
                        continue;

                    _logger.LogWriteLine($"Layer: {layerIndex} Epoch: {i}, Error: {error}");
                }
            }
        }

        public void RunTraining2(Training2Parameters parameters)
        {
            var trainingData = _configuration.InputsOutputsData;

            var teacher = new BackPropagationLearning(_neuralNetwork)
            {
                LearningRate = parameters.LearningRate,
                Momentum     = parameters.Momentum,
            };

            for (int i = 0; i < parameters.SupervisedEpochs; i++)
            {
                var error = teacher.RunEpoch(trainingData.Inputs, trainingData.Outputs) / trainingData.Inputs.Length;
                if (i % 10 != 0)
                    continue;

                _logger.LogWriteLine($"Supervised: {i}, Error = {error}");
            }
        }

        public void CheckAccuracy(InputOutputsDataNative testData)
        {
            var correctnessFactor = 0;

            var onePercent = (int)Math.Ceiling(testData.Inputs.Length / 100d);

            for (int i = 0; i < testData.Inputs.Length; i++)
            {
                var outputValues = _neuralNetwork.Compute(testData.Inputs[i]);

                var predicted = GetIndexOfResult(outputValues);
                var actual = GetIndexOfResult(testData.Outputs[i]);

                if (predicted == actual)
                    correctnessFactor++;

                if (i%onePercent == 0)
                    _logger.LogWriteLine($"Progress of computing correctness: {i * 100 / testData.Inputs.Length}%");
            }

            _logger.LogWriteLine($"Correct {Math.Round(correctnessFactor / (double)testData.Inputs.Length * 100, 2)}%");
        }

        private static int GetIndexOfResult(double[] output)
        {
            return output.ToList().IndexOf(output.Max());
        }
    }
}

using System;
using System.Linq;
using Accord.Math;
using Accord.Neuro.Learning;
using Accord.Neuro.Networks;
using AForge.Neuro.Learning;
using ImageClassification.Infrastructure;
using ImageClassification.Models;

namespace ImageClassification.Core
{
    public class Trainer
    {
        private readonly DeepBeliefNetwork _networkToTeach;
        private readonly TrainingData _trainingData;
        private readonly ILogger _logger;

        public Trainer(DeepBeliefNetwork networkToTeach, TrainingData trainingData, ILogger logger)
        {
            _networkToTeach = networkToTeach;
            _trainingData = trainingData;
            _logger = logger;
        }

        public void RunTraining1(Training1Parameters parameters)
        {
            var teacher = new DeepBeliefNetworkLearning(_networkToTeach)
            {
                Algorithm = (hiddenLayer, visibleLayer, i) => new ContrastiveDivergenceLearning(hiddenLayer, visibleLayer)
                {
                    LearningRate = parameters.LearningRate,
                    Momentum     = parameters.Momentum,
                    Decay        = parameters.Decay,
                }
            };

            var inputs = _trainingData.Inputs;

            // Setup batches of input for learning.
            var batchCount = Math.Max(1, inputs.Length / 100);

            // Create mini-batches to speed learning.
            var groups = Accord.Statistics.Tools.RandomGroups(inputs.Length, batchCount);
            var batches = inputs.Subgroups(groups);

            // Unsupervised learning on each hidden layer, except for the output layer.
            for (int layerIndex = 0; layerIndex < _networkToTeach.Machines.Count - 1; layerIndex++)
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
            var teacher = new BackPropagationLearning(_networkToTeach)
            {
                LearningRate = parameters.LearningRate,
                Momentum     = parameters.Momentum,
            };

            for (int i = 0; i < parameters.SupervisedEpochs; i++)
            {
                var error = teacher.RunEpoch(_trainingData.Inputs, _trainingData.Outputs) / _trainingData.Inputs.Length;
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
                var outputValues = _networkToTeach.Compute(testData.Inputs[i]);

                var predicted = GetResult(outputValues);
                var actual = GetResult(testData.Outputs[i]);

                if (predicted == actual)
                    correctnessFactor++;

                _logger.WriteLine($"predicted: {predicted} actual: {actual}");
            }

            _logger.WriteLine($"Correct {Math.Round(correctnessFactor / (double)testData.Inputs.Length * 100, 2)}%");
        }

        private static int GetResult(double[] output)
        {
            return output.ToList().IndexOf(output.Max());
        }
    }
}

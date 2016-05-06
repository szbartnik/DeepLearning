using System;
using System.Linq;
using Accord.Math;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Neuro.Networks;
using AForge.Neuro.Learning;
using NLog;
using Wkiro.ImageClassification.Core.Infrastructure.Logging;
using Wkiro.ImageClassification.Core.Models.Configurations;
using Wkiro.ImageClassification.Core.Models.Dto;
using Wkiro.ImageClassification.Core.Models.Requests;

namespace Wkiro.ImageClassification.Core.Engines
{
    internal class Trainer
    {
        private readonly TrainerConfiguration _configuration;
        private readonly SkipPhaseRequest _skipPhaseRequest;

        public DeepBeliefNetwork NeuralNetwork { get; }

        private IGuiLogger GuiLogger { get; }
        private ILogger NLogLogger { get; } = LogManager.GetCurrentClassLogger();

        public Trainer(
            TrainerConfiguration configuration, 
            SkipPhaseRequest skipPhaseRequest,
            IGuiLogger guiLogger)
        {
            NeuralNetwork = CreateNetworkToTeach(configuration);
            _configuration = configuration;
            _skipPhaseRequest = skipPhaseRequest;
            GuiLogger = guiLogger;
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
            LogInfoUsingBothLoggers("Started unsupervised training.");

            var teacher = new DeepBeliefNetworkLearning(NeuralNetwork)
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
            var guiLogIntensity = GetGuiLogIntensity(parameters.UnsupervisedEpochs);
            for (int layerIndex = 0; layerIndex < NeuralNetwork.Machines.Count - 1; layerIndex++)
            {
                teacher.LayerIndex = layerIndex;
                var layerData = teacher.GetLayerInput(batches);
                foreach (int i in Enumerable.Range(1, parameters.UnsupervisedEpochs))
                {
                    var error = teacher.RunEpoch(layerData) / inputs.Length;
                    var message = $"Layer: {layerIndex} Epoch: {i}, Error: {error}";
                    LogCurrentEpochResult(message, guiLogIntensity, i, parameters.UnsupervisedEpochs);

                    if (_skipPhaseRequest.RequestedAndUnhandled)
                    {
                        LogPhaseSkippnigAndNotifyHandled(i, parameters.UnsupervisedEpochs);
                        break;
                    }
                }
            }
        }

        private void LogCurrentEpochResult(
            string message, int guiLogIntensity,
            int currentEpoch, int plannedEpochCount
            )
        {
            bool shouldLogToGui = (currentEpoch % guiLogIntensity == 0) 
                || currentEpoch == plannedEpochCount;
            if (shouldLogToGui)
                GuiLogger.LogWriteLine(message);

            NLogLogger.Info(message);
        }

        private void LogPhaseSkippnigAndNotifyHandled(int currentEpoch, int plannedEpochCount)
        {
            var message = $"Skipping phase... {plannedEpochCount-currentEpoch} epochs skipped.";
            LogInfoUsingBothLoggers(message);
            _skipPhaseRequest.RequestedAndUnhandled = false;
        }  


        public void RunTraining2(Training2Parameters parameters)
        {
            LogInfoUsingBothLoggers("Started supervised training.");

            var trainingData = _configuration.InputsOutputsData;

            var teacher = new BackPropagationLearning(NeuralNetwork)
            {
                LearningRate = parameters.LearningRate,
                Momentum     = parameters.Momentum,
            };

            var guiLogIntensity = GetGuiLogIntensity(parameters.SupervisedEpochs);
            foreach (int i in Enumerable.Range(1, parameters.SupervisedEpochs))
            {
                var error = teacher.RunEpoch(trainingData.Inputs, trainingData.Outputs) / trainingData.Inputs.Length;
                var message = $"Supervised: {i}, Error = {error}";
                LogCurrentEpochResult(message, guiLogIntensity, i, parameters.SupervisedEpochs);

                if (_skipPhaseRequest.RequestedAndUnhandled)
                {
                    LogPhaseSkippnigAndNotifyHandled(i, parameters.SupervisedEpochs);
                    break;
                }
            }
        }

        private static int GetGuiLogIntensity(int epochsCount)
        {
            return epochsCount > 100
                ? 10
                : epochsCount > 20 ? 5 : 1;
        }

        public void CheckAccuracy(InputOutputsDataNative testData)
        {
            var accurateTestsCount = 0;
            var fivePercent = (int)Math.Ceiling(testData.Inputs.Length / 20d);

            for (int i = 0; i < testData.Inputs.Length; i++)
            {
                var outputValues = NeuralNetwork.Compute(testData.Inputs[i]);

                var predicted = GetIndexOfResult(outputValues);
                var actual = GetIndexOfResult(testData.Outputs[i]);

                if (predicted == actual)
                    accurateTestsCount++;



                if (i%fivePercent != 0)
                    continue;

                var message = $"Progress of evaluating accuracy: {i * 100 / testData.Inputs.Length}%";
                LogInfoUsingBothLoggers(message);
            }

            var accuracyMessage = $"Accuracy {Math.Round(accurateTestsCount / (double)testData.Inputs.Length * 100, 2)}%";
            LogInfoUsingBothLoggers(accuracyMessage);
        }

        private static int GetIndexOfResult(double[] output) => output.ToList().IndexOf(output.Max());

        private void LogInfoUsingBothLoggers(string message)
        {
            GuiLogger.LogWriteLine(message);
            NLogLogger.Info(message);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wkiro.ImageClassification.Core.Engines;
using Wkiro.ImageClassification.Core.Infrastructure.Logging;
using Wkiro.ImageClassification.Core.Models.Configurations;
using Wkiro.ImageClassification.Core.Models.Dto;
using Wkiro.ImageClassification.Core.Models.Requests;

namespace Wkiro.ImageClassification.Core.Facades
{
    public class LearningFacade
    {
        private readonly DataProvider _dataProvider;
        private readonly GlobalTrainerConfiguration _globalTrainerConfiguration;
        private readonly SkipPhaseRequest _skipPhaseRequest;
        private readonly IGuiLogger _guiLogger;

        public LearningFacade(
            DataProviderConfiguration dataProviderConfiguration, 
            GlobalTrainerConfiguration globalTrainerConfiguration, 
            SkipPhaseRequest skipPhaseRequest,
            IGuiLogger guiLogger)
        {
            _guiLogger = guiLogger;
            _dataProvider = new DataProvider(dataProviderConfiguration, globalTrainerConfiguration);
            _globalTrainerConfiguration = globalTrainerConfiguration;
            _skipPhaseRequest = skipPhaseRequest;
        }

        public IEnumerable<Category> GetAvailableCategories()
        {
            return _dataProvider.GetAvailableCategories();
        }

        public Task<ClassifierFacade> RunTrainingForSelectedCategories(TrainingParameters trainingParameters, CancellationToken ct)
        {
            return Task.Run(() =>
            {
                var t = Thread.CurrentThread;
                using (ct.Register(t.Abort))
                {
                    return RunTrainingForSelectedCategoriesImpl(trainingParameters);
                }
            }, ct);
        }

        private ClassifierFacade RunTrainingForSelectedCategoriesImpl(TrainingParameters trainingParameters)
        {
            var categories = trainingParameters.SelectedCategories.ToArray();
            var learningSet = _dataProvider.GetLearningSetForCategories(categories);

            var layers = _globalTrainerConfiguration.HiddenLayers.ToList();
            int outputLayerSize = categories.Length;
            layers.Add(outputLayerSize);

            var trainer = new Trainer(new TrainerConfiguration
            {
                Layers = layers.ToArray(),
                InputsOutputsData = learningSet.TrainingData.ToInputOutputsDataNative(),
            }, _skipPhaseRequest, _guiLogger);

            trainer.RunTraining1(trainingParameters.Training1Parameters);
            trainer.RunTraining2(trainingParameters.Training2Parameters);

            trainer.CheckAccuracy(learningSet.TestData.ToInputOutputsDataNative());

            var classifierConfiguration = new ClassifierConfiguration { Categories = categories };
            var classifier = new Classifier(trainer.NeuralNetwork, classifierConfiguration, _guiLogger);

            var classifierFacade = new ClassifierFacade(_dataProvider, classifier);
            return classifierFacade;
        }
    }
}

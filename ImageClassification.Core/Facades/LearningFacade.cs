using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wkiro.ImageClassification.Core.Engines;
using Wkiro.ImageClassification.Core.Infrastructure.Logging;
using Wkiro.ImageClassification.Core.Models.Configurations;
using Wkiro.ImageClassification.Core.Models.Dto;

namespace Wkiro.ImageClassification.Core.Facades
{
    public class LearningFacade
    {
        private readonly DataProvider _dataProvider;
        private readonly GlobalTrainerConfiguration _globalTrainerConfiguration;
        private readonly ILogger _logger;

        public LearningFacade(
            DataProviderConfiguration dataProviderConfiguration, 
            GlobalTrainerConfiguration globalTrainerConfiguration, 
            ILogger logger)
        {
            _dataProvider = new DataProvider(dataProviderConfiguration, globalTrainerConfiguration);
            _globalTrainerConfiguration = globalTrainerConfiguration;
            _logger = logger;
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
            var learningSet =
                    _dataProvider.GetLearningSetForCategories(trainingParameters.SelectedCategories.ToList());

            var trainer = new Trainer(new TrainerConfiguration
            {
                Layers = _globalTrainerConfiguration.Layers,
                InputsOutputsData = learningSet.TrainingData.ToInputOutputsDataNative(),
            }, _logger);

            trainer.RunTraining1(trainingParameters.Training1Parameters);
            trainer.RunTraining2(trainingParameters.Training2Parameters);

            trainer.CheckAccuracy(learningSet.TestData.ToInputOutputsDataNative());

            var classifier = new Classifier(trainer.NeuralNetwork, new ClassifierConfiguration
            {
                Categories = trainingParameters.SelectedCategories,
            }, _logger);

            var classifierFacade = new ClassifierFacade(_dataProvider, classifier);
            return classifierFacade;
        }
    }
}

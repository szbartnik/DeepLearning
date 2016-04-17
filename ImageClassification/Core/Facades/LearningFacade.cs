using System.Collections.Generic;
using Polsl.Inf.Os2.WKiRO.ImageClassification.Infrastructure.Logging;
using Polsl.Inf.Os2.WKiRO.ImageClassification.Models.Configurations;
using Polsl.Inf.Os2.WKiRO.ImageClassification.Models.Dto;

namespace Polsl.Inf.Os2.WKiRO.ImageClassification.Core.Facades
{
    public class LearningFacade
    {
        private readonly DataProvider _dataProvider;
        private readonly ILogger _logger;

        public LearningFacade(DataProviderConfiguration dataProviderConfiguration, ILogger logger)
        {
            _dataProvider = new DataProvider(dataProviderConfiguration);
            _logger = logger;
        }

        public IEnumerable<Category> GetAvailableCategories()
        {
            return _dataProvider.GetAvailableCategories();
        }

        public ClassifierFacade RunTrainingForSelectedCategories(TrainingParameters trainingParameters)
        {
            var learningSet = _dataProvider.GetLearningSetForCategories(trainingParameters.SelectedCategories);

            var trainer = new Trainer(new TrainerConfiguration
            {
                Layers = trainingParameters.Layers,
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

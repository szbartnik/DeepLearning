using System.Collections.Generic;
using Wkiro.ImageClassification.Core.Engines;
using Wkiro.ImageClassification.Core.Engines.PersistentStorage;
using Wkiro.ImageClassification.Core.Infrastructure.Logging;
using Wkiro.ImageClassification.Core.Models.Dto;

namespace Wkiro.ImageClassification.Core.Facades
{
    public class ClassifierFacade 
    {
        private readonly DataProvider _dataProvider;
        private readonly Classifier _classifier;
        private readonly IStorage _storage = new Storage();

        internal ClassifierFacade(
            DataProvider dataProvider, 
            Classifier classifier)
        {
            _classifier = classifier;
            _dataProvider = dataProvider;
        }

        public ClassifierFacade(string savedModelPath, IGuiLogger guiLogger)
        {
            var model = _storage.LoadModel(savedModelPath);
            _dataProvider = new DataProvider(model.DataProviderConfiguration);
            _classifier = new Classifier(model.Network, model.ClassifierConfiguration, guiLogger);
        }

        public CategoryClassification ClassifyToCategory(string imageToClassifyPath)
        {
            var preparedImage = _dataProvider.PrepareImageByPath(imageToClassifyPath);
            var classifiedCategory = _classifier.ClassifyToCategory(preparedImage);
            
            return classifiedCategory;
        }

        public void SaveModel(string saveLocationFilePath)
        {
            _storage.SaveModel(GetCurrentModel(), saveLocationFilePath);
        }

        public Model GetCurrentModel()
        {
            return new Model
            {
                Network = _classifier.Network,
                DataProviderConfiguration = _dataProvider.DataProviderConfiguration,
                ClassifierConfiguration = _classifier.ClassifierConfiguration
            };
        }
    }
}

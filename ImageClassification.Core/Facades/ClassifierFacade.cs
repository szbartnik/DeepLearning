using Wkiro.ImageClassification.Core.Engines;
using Wkiro.ImageClassification.Core.Models.Dto;

namespace Wkiro.ImageClassification.Core.Facades
{
    public class ClassifierFacade
    {
        private readonly DataProvider _dataProvider;
        private readonly Classifier _classifier;

        internal ClassifierFacade(DataProvider dataProvider, Classifier classifier)
        {
            _classifier = classifier;
            _dataProvider = dataProvider;
        }

        public CategoryClassification ClassifyToCategory(string imageToClassifyPath)
        {
            var preparedImage = _dataProvider.PrepareImageByPath(imageToClassifyPath);
            var classifiedCategory = _classifier.ClassifyToCategory(preparedImage);
            
            return classifiedCategory;
        }

        public void SaveClassifier(string saveLocationFilePath)
        {
            _classifier.SaveClassifier(saveLocationFilePath);
        }
    }
}

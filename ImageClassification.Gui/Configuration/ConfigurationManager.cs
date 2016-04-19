using Wkiro.ImageClassification.Core.Models.Configurations;

namespace Wkiro.ImageClassification.Gui.Configuration
{
    internal class ConfigurationManager : IConfigurationManager
    {
        public DataProviderConfiguration GetInitialDataProviderConfiguration()
        {
            var initialDataProviderConfiguration = new DataProviderConfiguration
            {
                CropWidth = 300,
                CropHeight = 200,
                ProcessingWidth = 30,
                ProcessingHeight = 20,
                TrainFilesLocationPath = @"C:\Users\Szymon\Desktop\101_ObjectCategories",
                FileExtensions = new[] { "jpg" },
                TrainDataRatio = 0.8,
            };

            return initialDataProviderConfiguration;
        }

        public Training1Parameters GetInitialTraining1Parameters()
        {
            var initialTraining1Parameters = new Training1Parameters
            {
                Momentum = 0.5,
                Decay = 0.001,
                LearningRate = 0.1,
                UnsupervisedEpochs = 200,
            };

            return initialTraining1Parameters;
        }

        public Training2Parameters GetInitialTraining2Parameters()
        {
            var initialTraining2Parameters = new Training2Parameters
            {
                Momentum = 0.5,
                LearningRate = 0.1,
                SupervisedEpochs = 300,
            };

            return initialTraining2Parameters;
        }
    }
}
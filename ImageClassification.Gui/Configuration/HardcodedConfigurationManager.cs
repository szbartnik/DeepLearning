using System.Windows;
using Wkiro.ImageClassification.Core.Models.Configurations;
using Wkiro.ImageClassification.Gui.Properties;

namespace Wkiro.ImageClassification.Gui.Configuration
{
    internal class HardcodedConfigurationManager : IConfigurationManager
    {
        public DataProviderConfiguration GetDataProviderConfiguration()
        {
            var initialDataProviderConfiguration = new DataProviderConfiguration
            {
                CropWidth = Settings.Default.DataProviderConfiguration_CropWidth,
                CropHeight = Settings.Default.DataProviderConfiguration_CropHeight,
                
                TrainFilesLocationPath = Settings.Default.DataProviderConfiguration_TrainFilesLocationPath,
                FileExtensions = Settings.Default.DataProviderConfiguration_FileExtensions.Split(',', ';', ' '),
            };

            return initialDataProviderConfiguration;
        }

        public GlobalTrainerConfiguration GetGlobalTrainerConfiguration()
        {
            return new GlobalTrainerConfiguration
            {
                HiddenLayers = new[] {600, 400, 2, 2},
                ProcessingWidth = 30,
                ProcessingHeight = 20,
                TrainDataRatio = 0.8,
            };
        }

        public Training1Parameters GetTraining1Parameters()
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

        public Training2Parameters GetTraining2Parameters()
        {
            var initialTraining2Parameters = new Training2Parameters
            {
                Momentum = 0.5,
                LearningRate = 0.1,
                SupervisedEpochs = 300,
            };

            return initialTraining2Parameters;
        }

        public void SaveConfigs(
            DataProviderConfiguration dataProviderConfiguration,
            GlobalTrainerConfiguration globalTrainerConfiguration,
            Training1Parameters training1Parameters,
            Training2Parameters training2Parameters)
        {
            
        }
    }
}
using System;
using System.Linq;
using Wkiro.ImageClassification.Core.Models.Configurations;
using Wkiro.ImageClassification.Gui.Properties;

namespace Wkiro.ImageClassification.Gui.Configuration
{
    internal class SettingsConfigurationManager : IConfigurationManager
    {
        #region Load area

        public DataProviderConfiguration GetDataProviderConfiguration()
        {
            var trainFilesLocationPath = Settings.Default.DataProviderConfiguration_TrainFilesLocationPath;

            var initialDataProviderConfiguration = new DataProviderConfiguration
            {
                ProcessingWidth = Settings.Default.DataProviderConfiguration_ProcessingWidth,
                ProcessingHeight = Settings.Default.DataProviderConfiguration_ProcessingHeight,

                TrainFilesLocationPath = string.IsNullOrEmpty(trainFilesLocationPath) ? Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) : trainFilesLocationPath,
                FileExtensions         = Settings.Default.DataProviderConfiguration_FileExtensions.Split(',', ';', ' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray(),
            };

            return initialDataProviderConfiguration;
        }

        public GlobalTrainerConfiguration GetGlobalTrainerConfiguration()
        {
            return new GlobalTrainerConfiguration
            {
                CropWidth = Settings.Default.GlobalTrainerConfiguration_CropWidth,
                CropHeight = Settings.Default.GlobalTrainerConfiguration_CropHeight,

                HiddenLayers     = Settings.Default.GlobalTrainerConfiguration_Layers.Split(',', ';', ' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToArray(),
                
                TrainDataRatio   = Settings.Default.GlobalTrainerConfiguration_TrainDataRatio,
            };
        }

        public Training1Parameters GetTraining1Parameters()
        {
            var initialTraining1Parameters = new Training1Parameters
            {
                Momentum           = Settings.Default.Training1Parameters_Momentum,
                Decay              = Settings.Default.Training1Parameters_Decay,
                LearningRate       = Settings.Default.Training1Parameters_LearningRate,
                UnsupervisedEpochs = Settings.Default.Training1Parameters_UnsupervisedEpochs,
            };

            return initialTraining1Parameters;
        }

        public Training2Parameters GetTraining2Parameters()
        {
            var initialTraining2Parameters = new Training2Parameters
            {
                Momentum         = Settings.Default.Training2Parameters_Momentum,
                LearningRate     = Settings.Default.Training2Parameters_LearningRate,
                SupervisedEpochs = Settings.Default.Training2Parameters_SupervisedEpochs,
            };

            return initialTraining2Parameters;
        }

        #endregion

        #region Save area

        public void SaveConfigs(
            DataProviderConfiguration dataProviderConfiguration,
            GlobalTrainerConfiguration globalTrainerConfiguration,
            Training1Parameters training1Parameters,
            Training2Parameters training2Parameters)
        {
            SaveDataProviderConfiguration(dataProviderConfiguration);
            SaveGlobalTrainerConfiguration(globalTrainerConfiguration);
            SaveTraining1Parameters(training1Parameters);
            SaveTraining2Parameters(training2Parameters);

            Settings.Default.Save();
        }

        private void SaveDataProviderConfiguration(DataProviderConfiguration dataProviderConfiguration)
        {
            Settings.Default["DataProviderConfiguration_ProcessingWidth"] = dataProviderConfiguration.ProcessingWidth;
            Settings.Default["DataProviderConfiguration_ProcessingHeight"] = dataProviderConfiguration.ProcessingHeight;

            Settings.Default["DataProviderConfiguration_TrainFilesLocationPath"] = dataProviderConfiguration.TrainFilesLocationPath;
            Settings.Default["DataProviderConfiguration_FileExtensions"]         = string.Join(";", dataProviderConfiguration.FileExtensions);
        }

        private void SaveGlobalTrainerConfiguration(GlobalTrainerConfiguration globalTrainerConfiguration)
        {
            if (globalTrainerConfiguration == null)
                return;

            Settings.Default["GlobalTrainerConfiguration_CropWidth"] = globalTrainerConfiguration.CropWidth;
            Settings.Default["GlobalTrainerConfiguration_CropHeight"] = globalTrainerConfiguration.CropHeight;

            

            Settings.Default["GlobalTrainerConfiguration_Layers"]           = string.Join(";", globalTrainerConfiguration.HiddenLayers);
            Settings.Default["GlobalTrainerConfiguration_TrainDataRatio"]   = globalTrainerConfiguration.TrainDataRatio;
        }

        private void SaveTraining1Parameters(Training1Parameters training1Parameters)
        {
            if (training1Parameters == null)
                return;

            Settings.Default["Training1Parameters_Momentum"]           = training1Parameters.Momentum;
            Settings.Default["Training1Parameters_Decay"]              = training1Parameters.Decay;
            Settings.Default["Training1Parameters_LearningRate"]       = training1Parameters.LearningRate;
            Settings.Default["Training1Parameters_UnsupervisedEpochs"] = training1Parameters.UnsupervisedEpochs;
        }

        private void SaveTraining2Parameters(Training2Parameters training2Parameters)
        {
            if (training2Parameters == null)
                return;

            Settings.Default["Training2Parameters_Momentum"]         = training2Parameters.Momentum;
            Settings.Default["Training2Parameters_LearningRate"]     = training2Parameters.LearningRate;
            Settings.Default["Training2Parameters_SupervisedEpochs"] = training2Parameters.SupervisedEpochs;
        }

        #endregion
    }
}
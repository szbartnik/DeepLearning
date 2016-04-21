using Wkiro.ImageClassification.Core.Models.Configurations;

namespace Wkiro.ImageClassification.Gui.Configuration
{
    internal interface IConfigurationManager
    {
        DataProviderConfiguration GetDataProviderConfiguration();
        Training1Parameters GetTraining1Parameters();
        Training2Parameters GetTraining2Parameters();
        GlobalTrainerConfiguration GetGlobalTrainerConfiguration();

        void SaveConfigs(
            DataProviderConfiguration dataProviderConfiguration,
            GlobalTrainerConfiguration globalTrainerConfiguration,
            Training1Parameters training1Parameters,
            Training2Parameters training2Parameters);
    }
}
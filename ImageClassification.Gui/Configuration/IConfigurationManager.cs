using Wkiro.ImageClassification.Core.Models.Configurations;

namespace Wkiro.ImageClassification.Gui.Configuration
{
    internal interface IConfigurationManager
    {
        DataProviderConfiguration GetInitialDataProviderConfiguration();
        Training1Parameters GetInitialTraining1Parameters();
        Training2Parameters GetInitialTraining2Parameters();
    }
}
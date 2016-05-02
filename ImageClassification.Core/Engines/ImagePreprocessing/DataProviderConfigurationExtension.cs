using System.Collections.Generic;
using System.Linq;
using Wkiro.ImageClassification.Core.Models.Configurations;

namespace Wkiro.ImageClassification.Core.Engines.ImagePreprocessing
{
    public static class DataProviderConfigurationExtension
    {
        public static IImagePreprocessingStrategy ToImagePreprocessingStrategy(
            this DataProviderConfiguration configuration)
        {
            var orderedStrategies = new List<IImagePreprocessingStrategy>();
            if(configuration.UseGrayScale)
                orderedStrategies.Add(new GrayScale());
            if(configuration.ShouldAutoCrop)
                orderedStrategies.Add(new AutoCrop(configuration));
            orderedStrategies.Add(new Scale(configuration));

            return orderedStrategies.Count == 1
                ? orderedStrategies.First()
                : new ChainedImagePreprocessing(orderedStrategies.ToArray());
        }
    }
}
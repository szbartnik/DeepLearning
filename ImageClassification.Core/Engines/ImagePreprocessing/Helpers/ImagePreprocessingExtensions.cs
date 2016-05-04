using System.Collections.Generic;
using System.Linq;
using Wkiro.ImageClassification.Core.Engines.ImagePreprocessing.Preprocessors;
using Wkiro.ImageClassification.Core.Models.Configurations;

namespace Wkiro.ImageClassification.Core.Engines.ImagePreprocessing.Helpers
{
    public static class ImagePreprocessingExtensions
    {
        public static IImagePreprocessor ToImagePreprocessingStrategy(
            this DataProviderConfiguration configuration)
        {
            var orderedStrategies = new List<IImagePreprocessor>();
            if(configuration.UseGrayScale)
                orderedStrategies.Add(new GrayScalePreprocessor());
            if(configuration.ShouldAutoCrop)
                orderedStrategies.Add(new AutoCropPreprocessor(configuration));
            if(configuration.ShouldEqualizeHistorgram)
                orderedStrategies.Add(new HistogramEqualizationPreprocessor());
            orderedStrategies.Add(new AutoScalePreprocessor(configuration));

            return orderedStrategies.Count == 1
                ? orderedStrategies.First()
                : new ImagePreprocessingPipe(orderedStrategies.ToArray());
        }
    }
}
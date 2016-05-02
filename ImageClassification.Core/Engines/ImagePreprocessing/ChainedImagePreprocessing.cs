using System.Drawing;
using Wkiro.ImageClassification.Core.Models.Configurations;

namespace Wkiro.ImageClassification.Core.Engines.ImagePreprocessing
{
    public class ChainedImagePreprocessing : IImagePreprocessingStrategy
    {
        private readonly IImagePreprocessingStrategy[] orderedStrategies;

        public ChainedImagePreprocessing(params IImagePreprocessingStrategy[] orderedStrategies)
        {
            this.orderedStrategies = orderedStrategies;
        }

        public Bitmap Process(Bitmap bitmap, DataProviderConfiguration configuration)
        {
            var processedBitmap = (Bitmap)bitmap.Clone();
            foreach (var strategy in orderedStrategies)
            {
                var nextBitmap = strategy.Process(processedBitmap, configuration);
                processedBitmap.Dispose();
                processedBitmap = nextBitmap;
            }
            return processedBitmap;
        }
    }
}
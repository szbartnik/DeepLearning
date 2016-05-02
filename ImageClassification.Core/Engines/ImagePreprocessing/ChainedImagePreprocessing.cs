using System.Drawing;

namespace Wkiro.ImageClassification.Core.Engines.ImagePreprocessing
{
    public class ChainedImagePreprocessing : IImagePreprocessingStrategy
    {
        private readonly IImagePreprocessingStrategy[] _orderedStrategies;

        public ChainedImagePreprocessing(params IImagePreprocessingStrategy[] orderedStrategies)
        {
            _orderedStrategies = orderedStrategies;
        }

        public Bitmap Process(Bitmap bitmap)
        {
            var processedBitmap = (Bitmap)bitmap.Clone();
            foreach (var strategy in _orderedStrategies)
            {
                var nextBitmap = strategy.Process(processedBitmap);
                processedBitmap.Dispose();
                processedBitmap = nextBitmap;
            }
            return processedBitmap;
        }
    }
}
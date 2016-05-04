using System.Drawing;

namespace Wkiro.ImageClassification.Core.Engines.ImagePreprocessing
{
    internal class ImagePreprocessingPipe : IImagePreprocessor
    {
        private readonly IImagePreprocessor[] _orderedStrategies;

        public ImagePreprocessingPipe(params IImagePreprocessor[] orderedStrategies)
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
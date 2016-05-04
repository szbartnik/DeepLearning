using System.Drawing;
using System.Drawing.Drawing2D;
using Wkiro.ImageClassification.Core.Models.Configurations;

namespace Wkiro.ImageClassification.Core.Engines.ImagePreprocessing.Preprocessors
{
    public class AutoScalePreprocessor : IImagePreprocessor
    {
        private readonly DataProviderConfiguration _configuration;

        public AutoScalePreprocessor(DataProviderConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Bitmap Process(Bitmap bitmap)
        {
            var newBitmap = new Bitmap(
                _configuration.ProcessingWidth,
                _configuration.ProcessingHeight);

            var graphics = Graphics.FromImage(newBitmap);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(bitmap, 0, 0, newBitmap.Width, newBitmap.Height);
            graphics.Dispose();

            return newBitmap;
        }
    }
}
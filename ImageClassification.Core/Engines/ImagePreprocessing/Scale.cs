using System.Drawing;
using Wkiro.ImageClassification.Core.Models.Configurations;

namespace Wkiro.ImageClassification.Core.Engines.ImagePreprocessing
{
    public class Scale : IImagePreprocessingStrategy
    {
        public Bitmap Process(Bitmap bitmap, DataProviderConfiguration configuration)
        {
            var newBitmap = new Bitmap(
                configuration.ProcessingWidth,
                configuration.ProcessingHeight);

            var graphics = Graphics.FromImage(newBitmap);
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(bitmap, 0, 0, newBitmap.Width, newBitmap.Height);
            graphics.Dispose();

            return newBitmap;
        }
    }
}
using System.Drawing;
using System.Drawing.Imaging;

namespace Wkiro.ImageClassification.Core.Engines.ImagePreprocessing.Preprocessors
{
    public class GrayScalePreprocessor : IImagePreprocessor
    {
        public Bitmap Process(Bitmap bitmap)
        {
            var newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            var colorMatrix = new ColorMatrix(new[]
            {
                new[] {.3f, .3f, .3f, 0f, 0f},
                new[] {.59f, .59f, .59f, 0f, 0f},
                new[] {.11f, .11f, .11f, 0f, 0f},
                new[] {0f, 0f, 0f, 1f, 0f},
                new[] {0f, 0f, 0f, 0f, 1f}
            });
            var attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);
            using (var graphics = Graphics.FromImage(newBitmap))
            {
                graphics.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, attributes);
            }
            return newBitmap;
        }
    }
}
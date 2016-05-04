using System.Drawing;

namespace Wkiro.ImageClassification.Core.Engines.ImagePreprocessing
{
    public interface IImagePreprocessor
    {
        Bitmap Process(Bitmap bitmap);
    }
}
using System.Drawing;

namespace Wkiro.ImageClassification.Core.Engines.ImagePreprocessing
{
    public interface IImagePreprocessingStrategy
    {
        Bitmap Process(Bitmap bitmap);
    }
}
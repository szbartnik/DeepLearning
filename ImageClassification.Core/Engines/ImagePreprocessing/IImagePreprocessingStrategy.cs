using System.Drawing;
using Wkiro.ImageClassification.Core.Models.Configurations;

namespace Wkiro.ImageClassification.Core.Engines.ImagePreprocessing
{
    public interface IImagePreprocessingStrategy
    {
        Bitmap Process(Bitmap bitmap, DataProviderConfiguration configuration);
    }
}
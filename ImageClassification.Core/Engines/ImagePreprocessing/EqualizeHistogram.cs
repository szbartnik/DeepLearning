using System.Drawing;
using AForge.Imaging.Filters;

namespace Wkiro.ImageClassification.Core.Engines.ImagePreprocessing
{
    public class EqualizeHistogram : IImagePreprocessingStrategy
    {
        public Bitmap Process(Bitmap bitmap)
        {
            var histogramEqualizer = new HistogramEqualization();
            return histogramEqualizer.Apply(bitmap);
        }
    }
}
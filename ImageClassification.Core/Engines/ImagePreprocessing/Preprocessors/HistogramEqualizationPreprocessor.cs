using System.Drawing;
using AForge.Imaging.Filters;

namespace Wkiro.ImageClassification.Core.Engines.ImagePreprocessing.Preprocessors
{
    internal class HistogramEqualizationPreprocessor : IImagePreprocessor
    {
        public Bitmap Process(Bitmap bitmap)
        {
            var histogramEqualizer = new HistogramEqualization();
            return histogramEqualizer.Apply(bitmap);
        }
    }
}
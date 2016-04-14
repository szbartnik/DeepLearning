using System;
using System.Linq;
using System.Windows.Forms;
using ImageClassification.Core;
using ImageClassification.Models.Configurations;

namespace ImageClassification
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            var dataProvider = new DataProvider(new DataProviderConfiguration
            {
                CropWidth = 30,
                CropHeight = 20,
                ProcessingWidth = 300,
                ProcessingHeight = 200,
                FilesLocationPath = @"C:\Users\Szymon\Desktop\101_ObjectCategories",
                FileExtensions = new [] { "jpg" },
                TrainDataRatio = 0.8,
            });

            var result = dataProvider.GetAvailableCategories();
            var result2 = dataProvider.GetLearningSetForCategories(result.Take(5).ToList());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}

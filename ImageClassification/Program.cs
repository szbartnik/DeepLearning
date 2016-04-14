using System;
using System.Linq;
using System.Windows.Forms;
using ImageClassification.Core;
using ImageClassification.Infrastructure;
using ImageClassification.Models.Configurations;
using ImageClassification.Models.Dto;

namespace ImageClassification
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            var dataProvider = new DataProvider(new DataProviderConfiguration
            {
                CropWidth = 300,
                CropHeight = 200,
                ProcessingWidth = 30,
                ProcessingHeight = 20,
                FilesLocationPath = @"C:\Users\Szymon\Desktop\101_ObjectCategories",
                FileExtensions = new [] { "jpg" },
                TrainDataRatio = 0.8,
            });

            var availableCategories = dataProvider.GetAvailableCategories();
            var selectedCategories = availableCategories.Take(5).ToList();

            var learningSet = dataProvider.GetLearningSetForCategories(selectedCategories);

            var trainer = new DeepLearningTrainer(new TrainerConfiguration
            {
                Layers = new []{ 600, 400, 5, 5},
                Categories = selectedCategories,
                InputsOutputsData = learningSet.TrainingData.ToInputOutputsDataNative(),
            }, new Logger());

            trainer.RunTraining1(new Training1Parameters
            {
                Momentum = 0.5,
                Decay = 0.001,
                LearningRate = 0.1,
                UnsupervisedEpochs = 200,
            });

            trainer.RunTraining2(new Training2Parameters
            {
                Momentum = 0.5,
                LearningRate = 0.1,
                SupervisedEpochs = 300,
            });

            trainer.CheckAccuracy(learningSet.TestData.ToInputOutputsDataNative());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Accord.Imaging.Converters;
using Wkiro.ImageClassification.Core.Engines.ImagePreprocessing.Helpers;
using Wkiro.ImageClassification.Core.Models.Configurations;
using Wkiro.ImageClassification.Core.Models.Dto;

namespace Wkiro.ImageClassification.Core.Engines
{
    internal class DataProvider
    {
        private readonly DataProviderConfiguration _dataProviderconfiguration;
        private readonly GlobalTrainerConfiguration _globalTrainerConfiguration;
        private readonly ImageToArray _imageToArray;

        public DataProviderConfiguration DataProviderConfiguration => _dataProviderconfiguration;

        public DataProvider(DataProviderConfiguration dataProviderconfiguration)
        {
            _dataProviderconfiguration = dataProviderconfiguration;
            _imageToArray = new ImageToArray(min: 0, max: 1);
        }

        public DataProvider(
            DataProviderConfiguration dataProviderConfiguration,
            GlobalTrainerConfiguration globalTrainerConfiguration)
            : this(dataProviderConfiguration)
        {
            _globalTrainerConfiguration = globalTrainerConfiguration;
        }

        public IEnumerable<Category> GetAvailableCategories()
        {
            var filesLocationPath = _dataProviderconfiguration.TrainFilesLocationPath;
            var categoriesFolders = Directory.GetDirectories(filesLocationPath);

            var itemCategoryEntries = categoriesFolders.Select((categoryFolderPath, i) =>
            {
                var categoryDirectoryInfo = new DirectoryInfo(categoryFolderPath);
                var filesOfCategory = GetFilesOfCategoryFolder(categoryDirectoryInfo);

                var category = new Category(
                    name:     categoryDirectoryInfo.Name,
                    fullPath: categoryFolderPath,
                    files:    filesOfCategory);

                return category;
            });

            return itemCategoryEntries;
        }

        public LearningSet GetLearningSetForCategories(Category[] categories)
        {
            var learningSet = new LearningSet();

            foreach (var category in categories)
            {
                var categoryInputsOutputs = GetCategoryLearningSet(category, categories.Length);
                learningSet.AddData(categoryInputsOutputs);
            }

            return learningSet;
        }

        private LearningSet SplitOnTrainAndTest(InputsOutputsData inputOutputsData)
        {
            var allSamplesCount = inputOutputsData.Count;
            var trainSamplesCount = GetTrainSampleCount(allSamplesCount);

            var rand = new Random();
            var learningSet = new LearningSet();
            var trainingData = learningSet.TrainingData;
            var testData = learningSet.TestData;

            for (int i = 0, trainSamplesLeft = trainSamplesCount, samplesLeft = allSamplesCount; 
                i < allSamplesCount; 
                ++i, --samplesLeft)
            {
                bool shouldTakeNextTrainSample = rand.Next(1, samplesLeft) <= trainSamplesLeft;
                if(shouldTakeNextTrainSample)
                {
                    trainingData.AddData(inputOutputsData.Inputs[i], inputOutputsData.Outputs[i]);
                    --trainSamplesLeft;
                }
                else
                {
                    testData.AddData(inputOutputsData.Inputs[i], inputOutputsData.Outputs[i]);
                }
            }

            return learningSet;
        }

        private int GetTrainSampleCount(int allSamplesCount)
        {
            var trainDataRatio = _globalTrainerConfiguration.TrainDataRatio;
            var trainSamplesCount = (int)Math.Round(trainDataRatio * allSamplesCount);

            return Math.Max(1, trainSamplesCount);
        }

        private LearningSet GetCategoryLearningSet(Category category, int numberOfCategories)
        {
            var inputOutputsData = new InputsOutputsData();
            var imagePreprocessingStrategy = _dataProviderconfiguration.ToImagePreprocessingStrategy();

            foreach (var file in category.Files)
            {
                using (var image = (Bitmap)Image.FromFile(file, true))
                using (var processedImage = imagePreprocessingStrategy.Process(image))
                {
                    var input = ConvertImage(processedImage);
                    inputOutputsData.Inputs.Add(input);
                    
                    var output = new double[numberOfCategories];
                    output[category.Index] = 1;
                    inputOutputsData.Outputs.Add(output);
                }
            }

            var splittedSets = SplitOnTrainAndTest(inputOutputsData);
            return splittedSets;
        }

        private FileInfo[] GetFilesOfCategoryFolder(DirectoryInfo categoryDirectory)
        {
            var filesExtensions = _dataProviderconfiguration.FileExtensions;
            var files = Enumerable.Empty<FileInfo>();

            foreach (var extension in filesExtensions)
            {
                files = files.Concat(categoryDirectory.GetFiles($"*.{extension}"));
            }

            return files.ToArray();
        }

        public double[] PrepareImageByPath(string imageFilePath)
        {
            var imagePreprocessingStrategy = _dataProviderconfiguration.ToImagePreprocessingStrategy();

            using (var image = (Bitmap)Image.FromFile(imageFilePath, true))
            using (var processedImage = imagePreprocessingStrategy.Process(image))
            {
                return ConvertImage(processedImage);
            }
        }

        private double[] ConvertImage(Bitmap image)
        {
            double[] converted;
            if (_dataProviderconfiguration.UseGrayScale)
                _imageToArray.Convert(image, out converted);
            else
                ExtractRgb(image, out converted);
            return converted;
        }

        private void ExtractRgb(Bitmap image, out double[] rgb)
        {
            double[][] argb;
            _imageToArray.Convert(image, out argb);

            var numberOfPixelsToSkip = argb[0].Length - 3; // Skipping alpha channel
            rgb = argb.SelectMany(x => x.Skip(numberOfPixelsToSkip)).ToArray();
        }
    }
}

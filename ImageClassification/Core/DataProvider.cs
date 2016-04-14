using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Accord.Imaging.Converters;
using ImageClassification.Models.Configurations;
using ImageClassification.Models.Dto;

namespace ImageClassification.Core
{
    public class DataProvider
    {
        private readonly DataProviderConfiguration _configuration;
        private readonly ImageToArray _imageToArray;
        private readonly ArrayToImage _arrayToImage;

        public DataProvider(DataProviderConfiguration configuration)
        {
            _configuration = configuration;
            _imageToArray = new ImageToArray(min: 0, max: 1);
            _arrayToImage = new ArrayToImage(
                configuration.ProcessingWidth, 
                configuration.ProcessingHeight, 
                min: 0.0, max: 1.0);
        }

        public IEnumerable<Category> GetAvailableCategories()
        {
            var filesLocationPath = _configuration.FilesLocationPath;
            var categoriesFolders = Directory.GetDirectories(filesLocationPath);

            var itemCategoryEntries = categoriesFolders.Select((categoryFolderPath, i) =>
            {
                var categoryDirectoryInfo = new DirectoryInfo(categoryFolderPath);
                var filesOfCategory = GetFilesOfCategoryFolder(categoryDirectoryInfo);

                var category = new Category(
                    index:      i,
                    name:       categoryDirectoryInfo.Name,
                    fullPath:   categoryFolderPath,
                    files:      filesOfCategory);

                return category;
            });

            return itemCategoryEntries;
        }

        public LearningSet GetLearningSetForCategories(List<Category> categories)
        {
            var learningSet = new LearningSet();

            foreach (var category in categories)
            {
                var categoryInputsOutputs = GetCategoryLearningSet(category, categories.Count);
                learningSet.AddData(categoryInputsOutputs);
            }

            return learningSet;
        }

        private LearningSet SplitOnTrainAndTest(InputsOutputsData inputOutputsData)
        {
            var numOfSamples = inputOutputsData.Count;
            var trainDataRatio = _configuration.TrainDataRatio;
            var trainSamplesNum = (int)Math.Round(trainDataRatio * numOfSamples);
            var testSamplesNum = numOfSamples - trainSamplesNum;

            var rand = new Random();

            var learningSet = new LearningSet();
            var trainingData = learningSet.TrainingData;
            var testData = learningSet.TestData;

            for (int i = 0; i < numOfSamples; i++)
            {
                if (trainingData.Count < trainSamplesNum && testData.Count < testSamplesNum)
                {
                    var randChoice = rand.Next(1);
                    if (randChoice == 1)
                        trainingData.AddData(inputOutputsData.Inputs[i], inputOutputsData.Outputs[i]);
                    else
                        testData.AddData(inputOutputsData.Inputs[i], inputOutputsData.Outputs[i]);

                    continue;
                }

                // Executed only if one training or test data set is full.
                if (trainingData.Count < trainSamplesNum)
                    trainingData.AddData(inputOutputsData.Inputs[i], inputOutputsData.Outputs[i]);
                else
                    testData.AddData(inputOutputsData.Inputs[i], inputOutputsData.Outputs[i]);
            }

            return learningSet;
        }

        private Bitmap ShrinkImage(Image bitmap)
        {
            var newBitmap = new Bitmap(_configuration.ProcessingWidth, _configuration.ProcessingHeight);

            var graphics = Graphics.FromImage(newBitmap);
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(bitmap, 0, 0, newBitmap.Width, newBitmap.Height);
            graphics.Dispose();

            return newBitmap;
        }

        private LearningSet GetCategoryLearningSet(Category category, int numberOfCategories)
        {
            var inputOutputsData = new InputsOutputsData();

            foreach (var file in category.Files)
            {
                var image = (Bitmap)Image.FromFile(file.FullName, true);
                if (image.Width < _configuration.CropWidth || image.Height < _configuration.CropHeight)
                    continue;

                // Crop the image
                image = image.Clone(new Rectangle(0, 0, _configuration.CropWidth, _configuration.CropHeight), image.PixelFormat);

                // Downsample the image to save memory
                var smallImage = ShrinkImage(image);
                image.Dispose();

                double[] input;
                _imageToArray.Convert(smallImage, out input);
                smallImage.Dispose();

                var output = new double[numberOfCategories];
                output[category.Index] = 1;

                inputOutputsData.Inputs.Add(input);
                inputOutputsData.Outputs.Add(output);
            }

            var splittedSets = SplitOnTrainAndTest(inputOutputsData);
            return splittedSets;
        }

        private FileInfo[] GetFilesOfCategoryFolder(DirectoryInfo categoryDirectory)
        {
            var filesExtensions = _configuration.FileExtensions;

            var files = Enumerable.Empty<FileInfo>();
            foreach (var extension in filesExtensions)
            {
                files = files.Concat(categoryDirectory.GetFiles($"*.{extension}"));
            }

            return files.ToArray();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Accord.Imaging.Converters;
using ImageClassification.Models;

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
                configuration.Width, 
                configuration.Height, 
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

        private FileInfo[] GetFilesOfCategoryFolder(DirectoryInfo dir)
        {
            var filesExtensions = _configuration.FileExtensions;

            var files = Enumerable.Empty<FileInfo>();
            foreach (var extension in filesExtensions)
            {
                files = files.Concat(dir.GetFiles(extension));
            }

            return files.ToArray();
        }
    }
}

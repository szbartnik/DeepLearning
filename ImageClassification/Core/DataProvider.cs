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

        public IEnumerable<Category> GetCategories()
        {
            var filesLocationPath = _configuration.FilesLocationPath;
            var categoriesFolders = Directory.GetDirectories(filesLocationPath);

            var itemCategoryEntries = categoriesFolders.Select((x, i) => new Category
            {
                Index = i,
                Name = new DirectoryInfo(x).Name,
                FullPath = x,
            });

            return itemCategoryEntries;
        } 
    }
}

namespace Wkiro.ImageClassification.Core.Models.Configurations
{
    public class DataProviderConfiguration
    {
        public string FilesLocationPath { get; set; }
        public string[] FileExtensions { get; set; }
        public double TrainDataRatio { get; set; }

        public int ProcessingWidth { get; set; }
        public int ProcessingHeight { get; set; }

        public int CropWidth { get; set; }
        public int CropHeight { get; set; }
    }
}

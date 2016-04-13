namespace ImageClassification.Models
{
    public class DataProviderConfiguration
    {
        public string FilesLocationPath { get; set; }
        public string[] FileExtensions { get; set; }   
        public int Width { get; set; }
        public int Height { get; set; }
    }
}

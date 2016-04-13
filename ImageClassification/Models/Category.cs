namespace ImageClassification.Models
{
    public class Category
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string FullPath { get; set; }

        public override string ToString()
        {
            return $"#{Index} - {Name}";
        }
    }
}
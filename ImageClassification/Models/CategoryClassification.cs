namespace ImageClassification.Models
{
    public class CategoryClassification
    {
        public Category Category { get; }
        public double Probability { get; }

        public CategoryClassification(Category category, double probability)
        {
            Category = category;
            Probability = probability;
        }
    }
}
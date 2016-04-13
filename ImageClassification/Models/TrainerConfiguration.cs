using System.Collections.Generic;

namespace ImageClassification.Models
{
    public class TrainerConfiguration
    {
        public List<Category> Categories { get; set; } 
        public TrainingData TrainingData { get; set; }
        public int[] Layers { get; set; }
    }
}
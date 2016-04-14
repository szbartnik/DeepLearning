namespace ImageClassification.Models.Configurations
{
    public class Training2Parameters
    {
        public double LearningRate { get; set; }
        public double Momentum { get; set; }
        public int SupervisedEpochs { get; set; }
    }
}
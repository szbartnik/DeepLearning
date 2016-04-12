namespace ImageClassification.Models
{
    public class Training1Parameters
    {
        public double LearningRate { get; set; }
        public double Momentum { get; set; }
        public int SupervisedEpochs { get; set; }
        public double Decay { get; set; }
    }
}
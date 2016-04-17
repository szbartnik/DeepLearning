namespace Wkiro.ImageClassification.Models.Configurations
{
    public class Training1Parameters
    {
        public double LearningRate { get; set; }
        public double Momentum { get; set; }
        public int UnsupervisedEpochs { get; set; }
        public double Decay { get; set; }
    }
}
namespace Wkiro.ImageClassification.Core.Models.Dto
{
    internal class LearningSet
    {
        public InputsOutputsData TrainingData { get; }
        public InputsOutputsData TestData { get; }

        public LearningSet()
        {
            TrainingData = new InputsOutputsData();
            TestData = new InputsOutputsData();
        }

        public void AddData(LearningSet categoryInputsOutputs)
        {
            TrainingData.AddData(categoryInputsOutputs.TrainingData);
            TestData.AddData(categoryInputsOutputs.TestData);
        }
    }
}

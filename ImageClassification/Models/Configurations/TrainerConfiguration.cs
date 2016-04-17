using Wkiro.ImageClassification.Models.Dto;

namespace Wkiro.ImageClassification.Models.Configurations
{
    public class TrainerConfiguration
    {
        public InputOutputsDataNative InputsOutputsData { get; set; }
        public int[] Layers { get; set; }
    }
}
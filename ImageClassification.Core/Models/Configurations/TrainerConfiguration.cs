using Wkiro.ImageClassification.Core.Models.Dto;

namespace Wkiro.ImageClassification.Core.Models.Configurations
{
    public class TrainerConfiguration
    {
        public InputOutputsDataNative InputsOutputsData { get; set; }
        public int[] Layers { get; set; }
    }
}
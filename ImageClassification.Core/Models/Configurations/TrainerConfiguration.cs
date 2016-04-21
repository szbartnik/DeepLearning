using Wkiro.ImageClassification.Core.Models.Dto;

namespace Wkiro.ImageClassification.Core.Models.Configurations
{
    internal class TrainerConfiguration
    {
        public InputOutputsDataNative InputsOutputsData { get; set; }
        public int[] Layers { get; set; }
    }
}
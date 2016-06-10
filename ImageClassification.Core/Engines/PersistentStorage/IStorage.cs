namespace Wkiro.ImageClassification.Core.Engines.PersistentStorage
{
    internal interface IStorage
    {
        void SaveModel(Model model, string filePath);
        Model LoadModel(string filePath);
    }
}

using Accord.Neuro.Networks;
using System.IO;
using System.IO.Compression;
using Wkiro.ImageClassification.Core.Engines.PersistentStorage.Helpers;
using Wkiro.ImageClassification.Core.Models.Configurations;

namespace Wkiro.ImageClassification.Core.Engines.PersistentStorage
{
    internal class Storage : IStorage
    {
        private const string NetworkFilename = "network";
        private const string ClassifierConfigurationFilename = "cc.xml";
        private const string DataProviderConfigurationFilename = "dpc.xml";

        public Model LoadModel(string filePath)
        {
            using (var tempDirectory = new TemporaryDirectory())
            {
                ZipFile.ExtractToDirectory(filePath, tempDirectory.Path);
                return new Model()
                {
                    Network = DeepBeliefNetwork.Load(GetNetworkPath(tempDirectory.Path)),
                    ClassifierConfiguration = Serializer.Load<ClassifierConfiguration>(
                        GetClassifierConfigurationPath(tempDirectory.Path)),
                    DataProviderConfiguration = Serializer.Load<DataProviderConfiguration>(
                        GetDataProviderConfigurationPath(tempDirectory.Path))
                };
            }
        }

        public void SaveModel(Model model, string filePath)
        {
            using (var tempDirectory = new TemporaryDirectory())
            { 
                SaveModelToTmpDirectory(model, tempDirectory.Path);
                ZipFile.CreateFromDirectory(tempDirectory.Path, filePath);
            }
        }

        private void SaveModelToTmpDirectory(Model model, string tempDirectory)
        {
            model.Network.Save(GetNetworkPath(tempDirectory));
            Serializer.Save(model.ClassifierConfiguration, 
                GetClassifierConfigurationPath(tempDirectory));
            Serializer.Save(model.DataProviderConfiguration, 
                GetDataProviderConfigurationPath(tempDirectory));
        }

        private string GetNetworkPath(string baseDirectory) 
            => Path.Combine(baseDirectory, NetworkFilename);

        private string GetClassifierConfigurationPath(string baseDirectory) 
            => Path.Combine(baseDirectory, ClassifierConfigurationFilename);

        private string GetDataProviderConfigurationPath(string baseDirectory)
            => Path.Combine(baseDirectory, DataProviderConfigurationFilename);

    }
}

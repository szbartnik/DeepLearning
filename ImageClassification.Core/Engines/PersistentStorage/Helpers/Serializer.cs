using System.IO;
using System.Xml.Serialization;

namespace Wkiro.ImageClassification.Core.Engines.PersistentStorage.Helpers
{
    internal static class Serializer
    {
        public static T Load<T>(string filePath)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var file = File.OpenText(filePath))
                return (T)serializer.Deserialize(file);
        }

        public static void Save<T>(T obj, string filePath)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var file = File.Create(filePath))
                serializer.Serialize(file, obj);
        }
    }
}

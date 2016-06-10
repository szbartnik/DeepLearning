using System.IO;
using System.Linq;

namespace Wkiro.ImageClassification.Core.Models.Dto
{
    public class Category
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string FullPath { get; set; }
        public string[] Files { get; set; }

        public Category()
        {
            // serializer requirement
        }

        public Category(string name, string fullPath, FileInfo[] files)
        {
            Name = name;
            FullPath = fullPath;
            Files = files.Select(x => x.FullName).ToArray();
        }

        public override string ToString()
        {
            return $"#{Index} - {Name}";
        }
    }
}
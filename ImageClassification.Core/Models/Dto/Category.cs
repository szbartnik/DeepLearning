using System.IO;

namespace Wkiro.ImageClassification.Core.Models.Dto
{
    public class Category
    {
        public int Index { get; set; }
        public string Name { get; }
        public string FullPath { get; }
        public FileInfo[] Files { get; }

        public Category(string name, string fullPath, FileInfo[] files)
        {
            Name = name;
            FullPath = fullPath;
            Files = files;
        }

        public override string ToString()
        {
            return $"#{Index} - {Name}";
        }
    }
}
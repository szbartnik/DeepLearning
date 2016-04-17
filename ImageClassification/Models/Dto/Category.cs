using System.IO;

namespace Polsl.Inf.Os2.WKiRO.ImageClassification.Models.Dto
{
    public class Category
    {
        public int Index { get; }
        public string Name { get; }
        public string FullPath { get; }
        public FileInfo[] Files { get; }

        public Category(int index, string name, string fullPath, FileInfo[] files)
        {
            Index = index;
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
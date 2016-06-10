using System;
using System.IO;
using SystemPath = System.IO.Path;

namespace Wkiro.ImageClassification.Core.Engines.PersistentStorage
{
    internal class TemporaryDirectory : IDisposable
    {
        public string Path { get; }

        public TemporaryDirectory()
        {
            Path = SystemPath.Combine(SystemPath.GetTempPath(), Guid.NewGuid().ToString());
            TryDelete();
            Directory.CreateDirectory(Path);
        }

        public void Dispose()
        {
            TryDelete();
        }

        private void TryDelete()
        {
            if (Directory.Exists(Path))
                Directory.Delete(Path, recursive: true);
        }
    }
}

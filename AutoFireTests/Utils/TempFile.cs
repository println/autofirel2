using System;
using System.IO;

namespace AutoFireTests.Utils
{
    public class TempFile : IDisposable
    {
        public string FilePath { get; private set; }

        public TempFile(string filename)
        {
            this.FilePath = GetTempFile(filename);
            CreateTempFile();
        }

        public TempFile() : this(Guid.NewGuid().ToString() + ".txt") { }

        private string GetTempFile(string filename)
        {
            return Path.Combine(Path.GetTempPath(), filename);
        }

        private void CreateTempFile()
        {
            File.Create(this.FilePath).Close();
        }

        private void RemoveTempFile()
        {
            File.Delete(FilePath);
        }

        public void Dispose()
        {
            RemoveTempFile();
        }
    }
}

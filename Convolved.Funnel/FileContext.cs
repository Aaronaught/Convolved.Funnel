using System;
using System.IO;

namespace Convolved.Funnel
{
    public class FileContext
    {
        public FileContext(string fileName, Stream stream)
        {
            Ensure.ArgumentNotNullOrEmpty(fileName, "fileName");
            Ensure.ArgumentNotNull(stream, "stream");
        }

        public string FileName { get; private set; }
        public Stream Stream { get; private set; }
    }
}
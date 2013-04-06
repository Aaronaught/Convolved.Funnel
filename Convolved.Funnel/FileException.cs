using System;

namespace Convolved.Funnel
{
    public class FileException : FunnelException
    {
        private const string DefaultMessage = "An error occurred while reading the file.";

        public FileException(string fileName = null, string message = DefaultMessage,
            Exception innerException = null)
        {
            this.FileName = fileName;
        }

        internal FileException(FileContext context, string message = DefaultMessage)
            : this(context.FileName, message)
        {
        }

        public string FileName { get; private set; }
    }
}
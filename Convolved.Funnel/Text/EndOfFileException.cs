using System;

namespace Convolved.Funnel.Text
{
    public class EndOfFileException : TextException
    {
        private const string DefaultMessage = "Attempted to read past the end of the file.";

        public EndOfFileException(string fileName = null, int lineNumber = 0, int linePosition = 0,
            string lineText = "", string message = DefaultMessage, Exception innerException = null)
            : base(fileName, lineNumber, linePosition, lineText, message, innerException)
        {
        }

        internal EndOfFileException(TextFileContext context, string message = DefaultMessage,
            Exception innerException = null)
            : base(context, message, innerException)
        {
        }
    }
}
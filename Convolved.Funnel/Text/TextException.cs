using System;

namespace Convolved.Funnel.Text
{
    public class TextException : FileException
    {
        private const string DefaultMessage = "An error occurred while reading the text file.";

        public TextException(string fileName = null, int lineNumber = 0, int linePosition = 0,
            string lineText = "", string message = DefaultMessage, Exception innerException = null)
            : base(fileName, message, innerException)
        {
            this.LineNumber = lineNumber;
            this.LinePosition = linePosition;
            this.LineText = lineText;
        }

        internal TextException(TextFileContext context, string message = DefaultMessage,
            Exception innerException = null)
            : this(context.FileName, context.CurrentLineNumber, context.CurrentLinePosition, 
            context.CurrentLine, message, innerException)
        {
        }

        public int LineNumber { get; private set; }
        public int LinePosition { get; private set; }
        public string LineText { get; private set; }
    }
}
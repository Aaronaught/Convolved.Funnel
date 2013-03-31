using System;

namespace Convolved.Funnel.Text
{
    public class EndOfLineException : TextException
    {
        private const string DefaultMessage = "Attempted to read past the end of the current line.";

        public EndOfLineException(string fileName = null, int lineNumber = 0, int linePosition = 0,
            string lineText = "", int characterCount = 1, string message = DefaultMessage,
            Exception innerException = null)
            : base(fileName, lineNumber, linePosition, lineText, message, innerException)
        {
            this.CharacterCount = characterCount;
        }

        internal EndOfLineException(TextFileContext context, int characterCount = 1,
            string message = DefaultMessage, Exception innerException = null)
            : base(context, message, innerException)
        {
            this.CharacterCount = characterCount;
        }

        public int CharacterCount { get; private set; }
    }
}
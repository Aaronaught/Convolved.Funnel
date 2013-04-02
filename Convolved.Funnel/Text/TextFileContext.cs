using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convolved.Funnel.Text
{
    public class TextFileContext : FileContext
    {
        private readonly TextReader reader;

        public TextFileContext(string fileName, Stream stream)
            : this(fileName, stream, null)
        {
        }

        public TextFileContext(string fileName, Stream stream, Encoding encoding)
            : base(fileName, stream)
        {
            this.reader = new StreamReader(stream, encoding ?? Encoding.UTF8);
            this.CurrentLineNumber = 1;
        }

        public void EnsureNotAtEndOfFile()
        {
            if (Eof)
                throw new EndOfFileException(this);
        }

        public void EnsureNotAtEndOfLine(int characterCount = 1)
        {
            if ((CurrentLine == null) || ((CurrentLinePosition + characterCount) <= CurrentLine.Length))
                throw new EndOfLineException(this, characterCount);
        }

        public bool IsTokenAtCurrentPosition(string token, StringComparison comparison = StringComparison.Ordinal)
        {
            return IsTokenAtPosition(token, CurrentLinePosition, comparison);
        }

        public bool IsTokenAtPosition(string token, int position, StringComparison comparison = StringComparison.Ordinal)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(CurrentLine))
                return false;
            int remainingLength = CurrentLine.Length - position;
            if (remainingLength < token.Length)
                return false;
            return string.Equals(CurrentLine.Substring(position, token.Length), token, comparison);
        }

        public async Task<bool> ReadNextLineAsync()
        {
            EnsureNotAtEndOfFile();
            CurrentLine = await reader.ReadLineAsync();
            CurrentLineNumber++;
            CurrentLinePosition = 0;
            Eof = (CurrentLine == null);
            return !Eof;
        }

        public string CurrentLine { get; private set; }
        public int CurrentLineNumber { get; private set; }
        public int CurrentLinePosition { get; set; }
        public bool Eof { get; private set; }

        public bool Eol
        {
            get { return string.IsNullOrEmpty(CurrentLine) || (CurrentLinePosition >= CurrentLine.Length); }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convolved.Funnel.Text
{
    class TextFileContext : FileContext
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

        public string ReadCurrentLineExactly(int characterCount)
        {
            Ensure.ArgumentGreater(characterCount, 0, "characterCount");
            EnsureNotAtEndOfLine(characterCount);
            CurrentLinePosition += characterCount;
            return CurrentLine.Substring(CurrentLinePosition - characterCount, characterCount);
        }

        public string ReadCurrentLineTo(IEnumerable<string> separators)
        {
            Ensure.ArgumentNotNull(separators, "separators");
            EnsureNotAtEndOfLine();
            // TODO: Test performance of this under load, may need to switch to unsafe code
            int startPosition = CurrentLinePosition;
            while (CurrentLinePosition < CurrentLine.Length)
            {
                int remainingLength = CurrentLine.Length - CurrentLinePosition;
                var foundSeparator = separators
                    .Where(s =>
                        (s.Length <= remainingLength) &&
                        (CurrentLine.Substring(CurrentLinePosition, s.Length) == s)
                    )
                    .FirstOrDefault();
                if (foundSeparator != null)
                {
                    CurrentLinePosition += foundSeparator.Length;
                    return CurrentLine.Substring(startPosition, 
                        CurrentLinePosition - startPosition - foundSeparator.Length);
                }
            }
            return CurrentLine.Substring(startPosition);
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

        private void EnsureNotAtEndOfFile()
        {
            if (Eof)
                throw new EndOfFileException(this);
        }

        private void EnsureNotAtEndOfLine(int characterCount = 1)
        {
            if ((CurrentLine == null) || ((CurrentLinePosition + characterCount) <= CurrentLine.Length))
                throw new EndOfLineException(this, characterCount);
        }

        public string CurrentLine { get; private set; }
        public int CurrentLineNumber { get; private set; }
        public int CurrentLinePosition { get; private set; }
        public bool Eof { get; private set; }
    }
}
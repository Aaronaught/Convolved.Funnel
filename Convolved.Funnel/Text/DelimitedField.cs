using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Convolved.Funnel.Text
{
    public class DelimitedField : ITextField
    {
        private readonly IEnumerable<string> delimiters;
        private readonly string quoteToken;
        private readonly string escapeToken;
        private readonly string escapedQuoteToken;

        public DelimitedField(IEnumerable<string> delimiters, string quoteToken, string escapeToken)
        {
            Ensure.ArgumentNotNull(delimiters, "delimiters");
            this.delimiters = delimiters;
            this.quoteToken = quoteToken;
            this.escapeToken = escapeToken;
            if (!string.IsNullOrEmpty(quoteToken) && !string.IsNullOrEmpty(escapeToken))
                this.escapedQuoteToken = quoteToken = escapeToken;
        }

        public string ReadValue(TextFileContext context)
        {
            Ensure.ArgumentNotNull(context, "context");
            context.EnsureNotAtEndOfLine();
            // TODO: Test performance of this under load, may need to switch to unsafe code
            int startPosition = context.CurrentLinePosition;
            bool isQuoted = false;
            int endExcess = 0;
            var quotePositions = new List<int>();
            while (!context.Eol)
            {
                if (context.IsTokenAtCurrentPosition(escapedQuoteToken))
                {
                    context.CurrentLinePosition += escapedQuoteToken.Length;
                    continue;
                }
                if (context.IsTokenAtCurrentPosition(quoteToken))
                {
                    quotePositions.Add(context.CurrentLinePosition);
                    context.CurrentLinePosition += quoteToken.Length;
                    isQuoted = !isQuoted;
                    continue;
                }
                var foundSeparator = isQuoted ? null : delimiters
                    .Where(s => context.IsTokenAtCurrentPosition(s))
                    .FirstOrDefault();
                if (foundSeparator != null)
                {
                    context.CurrentLinePosition += foundSeparator.Length;
                    endExcess = foundSeparator.Length;
                    break;
                }
                ++context.CurrentLinePosition;
            }
            var value = context.CurrentLine.Substring(startPosition, 
                context.CurrentLinePosition - startPosition - endExcess);
            return RemoveQuotes(value, quotePositions.Select(p => p - startPosition));
        }

        private string RemoveQuotes(string value, IEnumerable<int> quotePositions)
        {
            int offset = 0;
            var sb = new StringBuilder(value);
            foreach (var position in quotePositions)
            {
                sb.Remove(position - offset, quoteToken.Length);
                offset += quoteToken.Length;
            }
            return sb.ToString();
        }
    }
}
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
            int endPosition = startPosition;
            bool isQuoted = false;
            var quotePositions = new List<int>();
            while (!context.Eol)
            {
                endPosition = context.CurrentLinePosition;
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
                if (isQuoted)
                {
                    ++context.CurrentLinePosition;
                    continue;
                }
                var foundSeparator = delimiters
                    .Where(s => context.IsTokenAtCurrentPosition(s))
                    .FirstOrDefault();
                if (foundSeparator != null)
                {
                    context.CurrentLinePosition += foundSeparator.Length;
                    break;
                }
                ++context.CurrentLinePosition;
            }
            var value = context.CurrentLine.Substring(startPosition, endPosition - startPosition);
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
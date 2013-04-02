using System;

namespace Convolved.Funnel.Text
{
    public class FixedWidthField : ITextField
    {
        private readonly int size;

        public FixedWidthField(int size)
        {
            Ensure.ArgumentGreater(size, 0, "size");
            this.size = size;
        }

        public string ReadValue(TextFileContext context)
        {
            Ensure.ArgumentNotNull(context, "context");
            context.EnsureNotAtEndOfLine(size);
            context.CurrentLinePosition += size;
            return context.CurrentLine.Substring(context.CurrentLinePosition - size, size);
        }
    }
}
using System;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureQuoting<T> : IConfigureText<T>
    {
        IConfigureQuoting<T> AutoDetectQuotedFields();
        IConfigureQuoting<T> With(string quoteToken);
        IConfigureQuoting<T> EscapeWith(string escapeToken);
    }
}
using System;
using System.Collections.Generic;
using Convolved.Funnel.Text;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureText<T> : IConfigure<T>
    {
        IConfigureTextSection<T, T> Body();
        IConfigureText<T> Convention<TProperty>(Func<string, TProperty> parse);
        IConfigureText<T> Convention<TProperty, TParser>() where TParser : IParse<TProperty>;
        IConfigureText<T> DefaultDelimiter(string delimiter);
        IConfigureText<T> IgnoreBlankLines();
        IConfigureQuoting<T> Quoting();
        IConfigureTextSection<T, TSection> Section<TSection>(Func<T, TSection> selector);
        IConfigureTextSection<T, TSection> Section<TSection>(Func<T, IEnumerable<TSection>> selector);
        IConfigureTextSection<T, TSection> Section<TSection>(Func<T, IList<TSection>> selector);
        IConfigureTextSection<T, TSection> Section<TSection>(Func<T, ICollection<TSection>> selector);
        IConfigureParseErrorHandling<T> ParseErrors { get; }
    }
}
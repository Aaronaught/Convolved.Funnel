using System;
using System.Linq.Expressions;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureFields<T, TSection> : IConfigureText<T>
    {
        IConfigureFields<T, TSection> Delimited(params Expression<Func<TSection, object>>[] properties);
        IConfigureFields<T, TSection> Delimited(string delimiter, params Expression<Func<TSection, object>>[] properties);
        IConfigureFields<T, TSection> Delimited(string[] delimiters, params Expression<Func<TSection, object>>[] properties);
        IConfigureFields<T, TSection> Fixed(uint length, params Expression<Func<TSection, object>>[] properties);
        IConfigureFields<T, TSection> QuotedDelimited(string quoteToken, string escapeToken,
            params Expression<Func<TSection, object>>[] properties);
        IConfigureFields<T, TSection> QuotedDelimited(string delimiter, string quoteToken, string escapeToken,
            params Expression<Func<TSection, object>>[] properties);
        IConfigureFields<T, TSection> QuotedDelimited(string[] delimiters, string quoteToken, string escapeToken,
            params Expression<Func<TSection, object>>[] properties);
        IConfigureFields<T, TSection> RecordType<TRecord>(Expression<Func<TSection, object>> discriminatorExpression, 
            object discriminatorValue);
        IConfigureUnknownRecordType<T, TSection> UnknownRecordType { get; }
    }
}
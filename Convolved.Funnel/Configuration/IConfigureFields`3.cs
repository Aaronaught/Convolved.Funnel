using System;
using System.Linq.Expressions;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureFields<T, TSection, TRecord> : IConfigureText<T>
    {
        IConfigureFields<T, TSection, TRecord> Custom<TProperty>(Expression<Func<TRecord, TProperty>> property, Func<string, TProperty> valueFromLineSelector);
        IConfigureFields<T, TSection, TRecord> Delimited(params Expression<Func<TRecord, object>>[] properties);
        IConfigureFields<T, TSection, TRecord> Delimited(string delimiter, params Expression<Func<TRecord, object>>[] properties);
        IConfigureFields<T, TSection, TRecord> Delimited(string[] delimiters, params Expression<Func<TRecord, object>>[] properties);
        IConfigureFields<T, TSection, TRecord> Delimited<TProperty>(string delimiter, Expression<Func<TRecord, TProperty>> property, Func<string, TProperty> parse);
        IConfigureFields<T, TSection, TRecord> Delimited<TProperty>(string[] delimiters, Expression<Func<TRecord, TProperty>> property, Func<string, TProperty> parse);
        IConfigureFields<T, TSection, TRecord> Fixed(uint length, params Expression<Func<TRecord, object>>[] properties);
        IConfigureFields<T, TSection, TRecord> Fixed<TProperty>(uint length, Expression<Func<TRecord, TProperty>> property, Func<string, TProperty> parse);
        IConfigureFields<T, TSection, TRecord> QuotedDelimited(params Expression<Func<TRecord, object>>[] properties);
        IConfigureFields<T, TSection, TRecord> QuotedDelimited(string quoteToken, string escapeToken, params Expression<Func<TRecord, object>>[] properties);
        IConfigureFields<T, TSection, TRecord> QuotedDelimited(string delimiter, string quoteToken, string escapeToken, params Expression<Func<TRecord, object>>[] properties);
        IConfigureFields<T, TSection, TRecord> QuotedDelimited(string[] delimiters, string quoteToken, string escapeToken, params Expression<Func<TRecord, object>>[] properties);
        IConfigureFields<T, TSection, TRecord> RecordType<TRecord>(Expression<Func<TRecord, object>> discriminatorExpression, object discriminatorValue);
        IConfigureFields<T, TSection, TSection> AnyRecordType { get; }
        IConfigureUnknownRecordType<T, TSection> UnknownRecordType { get; }
    }
}
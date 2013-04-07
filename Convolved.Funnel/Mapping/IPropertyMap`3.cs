using System;

namespace Convolved.Funnel.Mapping
{
    public interface IPropertyMap<T, TContext>
        where TContext : FileContext
    {
        void Extract(TContext context, T target);
    }
}
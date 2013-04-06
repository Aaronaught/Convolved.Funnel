using System;

namespace Convolved.Funnel.Mapping
{
    public interface IClassMap<T, TContext>
        where TContext : FileContext
    {
        void Extract(TContext context, T target);
    }
}

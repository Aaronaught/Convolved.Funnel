using System;
using System.Threading.Tasks;

namespace Convolved.Funnel.Mapping
{
    public interface IClassMap<T, TContext>
        where TContext : FileContext
    {
        Task ExtractAsync(TContext context, T target);
    }
}

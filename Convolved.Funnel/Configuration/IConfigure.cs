using System;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigure : IFluentInterface
    {
        IConfigure<T> As<T>();
    }
}
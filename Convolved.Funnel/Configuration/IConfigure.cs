using System;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigure
    {
        IConfigure<T> As<T>();
    }
}
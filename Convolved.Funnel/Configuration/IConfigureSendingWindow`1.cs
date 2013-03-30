using System;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureSendingWindow<T> : IFluentInterface
    {
        IConfigureSending<T> Milliseconds();
        IConfigureSending<T> Seconds();
        IConfigureSending<T> Minutes();
    }
}
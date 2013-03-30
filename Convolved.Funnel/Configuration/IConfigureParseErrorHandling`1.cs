using System;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureParseErrorHandling<T> : IConfigureErrorHandling<T>
    {
        IConfigureParseErrorHandling<T> LeavePlaceholder();
    }
}
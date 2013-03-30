using System;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureParseErrorHandling<T> : IConfigureErrorHandling<T>
    {
        IConfigureText<T> Catch<T>() where T : Exception;
        IConfigureParseErrorHandling<T> LeavePlaceholder();
    }
}
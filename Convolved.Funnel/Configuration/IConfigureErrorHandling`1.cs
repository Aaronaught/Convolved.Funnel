using System;
using Convolved.Funnel.Tasks;
using Convolved.Funnel.Validation;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureErrorHandling<T> : IConfigure<T>
    {
        IConfigureErrorHandling<T> HandleWith(Action<ValidationError> handler);
        IConfigureErrorHandling<T> HandleWith(IHandleErrors handler);
        IConfigureErrorHandling<T> HandleWith(IHandleErrors<ValidationError> handler);
        IConfigureErrorHandling<T> HandleWith<THandler>(params object[] args) 
            where THandler : IHandleErrors;
        IConfigureErrorHandling<T> Throw();
    }
}
using System;
using Convolved.Funnel.Validation;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureValidation<T> : IConfigureErrorHandling<T>
    {
        IConfigureValidation<T> ValidateWith(IValidate<T> validator);
        IConfigureValidation<T> ValidateWith<TValidator>(params object[] parameters)
            where TValidator : IValidate<T>;
    }
}
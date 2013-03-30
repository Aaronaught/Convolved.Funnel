using System;
using Convolved.Funnel.Validation;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureValidation<T> : IConfigureErrorHandling<T>
    {
        IConfigure<T> ValidateWith<TValidator>() where TValidator : IValidate<T>;
    }
}
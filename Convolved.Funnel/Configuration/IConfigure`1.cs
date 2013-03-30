using System;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigure<T> : IFluentInterface
    {
        IConfigureText<T> Text();
        IConfigureValidation<T> Validation();
    }
}
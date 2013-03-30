using System;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigure<T>
    {
        IConfigureText<T> Text();
        IConfigureValidation<T> Validation();
    }
}
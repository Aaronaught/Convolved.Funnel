using System;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureTextSectionExactly<T, TSection> : IFluentInterface
    {
        IConfigureTextSection<T, TSection> Lines();
    }
}
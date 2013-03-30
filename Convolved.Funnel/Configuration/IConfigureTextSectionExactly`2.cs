using System;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureTextSectionExactly<T, TSection>
    {
        IConfigureTextSection<T, TSection> Lines();
    }
}
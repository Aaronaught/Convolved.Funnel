using System;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureUnknownRecordType<T, TSection> : IConfigureFields<T, TSection>
    {
        IConfigureFields<T, TSection> Error();
        IConfigureFields<T, TSection> Ignore();
    }
}
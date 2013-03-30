using System;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureUnknownRecordType<T, TSection> : IFluentInterface
    {
        IConfigureFields<T, TSection, TSection> Error();
        IConfigureFields<T, TSection, TSection> Ignore();
    }
}
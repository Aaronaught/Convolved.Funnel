using System;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureReportWindow<T> : IFluentInterface
    {
        IConfigureReportSettings<T> Seconds();
        IConfigureReportSettings<T> Minutes();
        IConfigureReportSettings<T> Hours();
        IConfigureReportSettings<T> Days();
    }
}
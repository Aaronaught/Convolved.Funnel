using System;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureReportSettings<T> : IConfigureReports<T>
    {
        IConfigureReportWindow<T> Every(uint timeUnits);
        IConfigureReportSettings<T> PreferredTime(TimeSpan timeOfDay);
        IConfigureReportSettings<T> SendTo(params string[] recipients);
        IConfigureReportSettings<T> SuppressIfEmpty();
    }
}
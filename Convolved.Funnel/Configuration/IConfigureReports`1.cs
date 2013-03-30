using System;
using Convolved.Funnel.Tasks;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureReports<T> : IConfigureTasks<T>
    {
        IConfigureReportSettings<T> Errors();
        IConfigureReportSettings<T> NoFilesReceived();
        IConfigureReportSettings<T> Summary();
        IConfigureReports<T> SendAllTo(params string[] recipients);
        IConfigureReports<T> SendWith(ISendReports sender);
        IConfigureReports<T> SendWith<TSender>(params object[] arguments)
            where TSender : ISendReports;
    }
}
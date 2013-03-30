using System;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureSending<T> : IConfigureTasks<T>
    {
        IConfigureSending<T> InBatchesOf(uint batchSize);
        IConfigureSendingWindow<T> Every(uint timeUnits);
    }
}
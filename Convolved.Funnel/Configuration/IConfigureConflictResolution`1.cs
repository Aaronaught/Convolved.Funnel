using System;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureConflictResolution<T> : IFluentInterface
    {
        IConfigureTasks<T> Delete();
        IConfigureConflictResolution<T> DeleteIfOlder();
        IConfigureTasks<T> Rename();
        IConfigureTasks<T> Rename(uint maxCopies);
        IConfigureTasks<T> Replace();
        IConfigureConflictResolution<T> ReplaceIfNewer();
    }
}
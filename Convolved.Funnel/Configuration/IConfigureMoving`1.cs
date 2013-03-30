using System;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureMoving<T> : IConfigureTasks<T>
    {
        IConfigureMoving<T> To(string directory);
        IConfigureConflictResolution<T> ConflictResolution { get; }
    }
}
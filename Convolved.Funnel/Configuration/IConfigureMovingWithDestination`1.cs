using System;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureMovingWithDestination<T> : IConfigureTasks<T>
    {
        IConfigureConflictResolution<T> ConflictResolution { get; }
    }
}
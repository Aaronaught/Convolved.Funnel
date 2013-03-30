using System;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureMoving<T> : IConfigureTasks<T>
    {
        IConfigureTasks<T> To(string directory);
    }
}
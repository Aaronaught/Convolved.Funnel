using System;
using Convolved.Funnel.Tasks;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureTasks<T>
    {
        IConfigureMoving<T> Delete();
        IConfigureMoving<T> DeleteIf(FileStatus status);
        IConfigureMoving<T> Move();
        IConfigureMoving<T> MoveIf(FileStatus status);
        IConfigureSending<T> SendTo(Action<T> receiver);
        IConfigureSending<T> SendTo(IReceive<T> receiver);
        IConfigureSending<T> SendTo<TReceiver>(params object[] args)
            where TReceiver : IReceive<T>;
        IConfigureTasks<T> TransformWith(Action<T> transform);
        IConfigureTasks<T> TransformWith(ITransform<T> transform);
        IConfigureTasks<T> TransformWith<TTransform>(params object[] parameters)
            where TTransform : ITransform<T>;
    }
}
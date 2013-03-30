using System;

namespace Convolved.Funnel.Tasks
{
    public interface IHandleErrors<TError>
        where TError : Error
    {
        void Handle(TError error);
    }
}
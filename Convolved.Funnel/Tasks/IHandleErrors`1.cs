using System;

namespace Convolved.Funnel.Tasks
{
    public interface IHandleErrors<in TError> : IHandleErrors
        where TError : Error
    {
        void Handle(TError error);
    }
}
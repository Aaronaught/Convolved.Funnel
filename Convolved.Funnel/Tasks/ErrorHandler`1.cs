using System;

namespace Convolved.Funnel.Tasks
{
    public abstract class ErrorHandler<TError> : IHandleErrors<TError>
        where TError : Error
    {
        public abstract void Handle(TError error);

        public void Handle(Error error)
        {
            var handledError = error as TError;
            if (handledError != null)
                Handle(handledError);
        }
    }
}
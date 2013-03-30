using System;

namespace Convolved.Funnel.Tasks
{
    public interface IHandleErrors
    {
        void Handle(Error error);
    }
}
using System;
using System.Collections.Generic;

namespace Convolved.Funnel.Tasks
{
    public interface IReceive<T>
    {
        void Receive(IEnumerable<T> items);
    }
}
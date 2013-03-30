using System;

namespace Convolved.Funnel.Tasks
{
    public interface ITransform<T>
    {
        void Transform(T model);
    }
}
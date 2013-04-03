using System;
using System.Collections.Concurrent;

namespace Convolved.Funnel.Mapping
{
    public static class PropertyMapper
    {
        private static readonly ConcurrentDictionary<Type, object> mappers = new ConcurrentDictionary<Type, object>();

        public static PropertyMapper<T> Get<T>()
        {
            return (PropertyMapper<T>)mappers.GetOrAdd(typeof(T), t => new PropertyMapper<T>());
        }
    }
}
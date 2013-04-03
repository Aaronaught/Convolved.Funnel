using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Convolved.Funnel.Mapping
{
    public class PropertyMapper<T>
    {
        private readonly Dictionary<string, Func<T, object>> getters = new
            Dictionary<string, Func<T, object>>();
        private readonly Dictionary<string, Action<T, object>> setters = new 
            Dictionary<string, Action<T, object>>();

        public PropertyMapper()
        {
            var propertiesToMap = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.GetIndexParameters().Length == 0);
            foreach (var property in propertiesToMap)
                Map(property.Name, property.PropertyType);
        }

        public object GetValue(T instance, string propertyName)
        {
            Func<T, object> getter;
            if (getters.TryGetValue(propertyName, out getter))
                return getter(instance);
            throw new PropertyNotMappedException(typeof(T), propertyName);
        }

        public void SetValue(T instance, string propertyName, object value)
        {
            Action<T, object> setter;
            if (setters.TryGetValue(propertyName, out setter))
                setter(instance, value);
            else
                throw new PropertyNotMappedException(typeof(T), propertyName);
        }

        private void Map(string propertyName, Type propertyType)
        {
            var objectParameter = Expression.Parameter(typeof(T), "obj");
            var getter =
                Expression.Lambda<Func<T, object>>(
                    Expression.Convert(
                        Expression.Property(objectParameter, propertyName),
                        typeof(object)
                    ),
                    objectParameter
                )
                .Compile();
            getters.Add(propertyName, getter);
            var valueParameter = Expression.Parameter(typeof(object), "v");
            var setter = 
                Expression.Lambda<Action<T, object>>(
                    Expression.Assign(
                        Expression.Property(objectParameter, "Id"),
                        Expression.Convert(valueParameter, propertyType)
                    ),
                    objectParameter,
                    valueParameter
                )
                .Compile();
            setters.Add(propertyName, setter);
        }
    }
}
using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Convolved.Funnel.Mapping
{
    public static class PropertyMapper<T>
    {
        private static readonly ConcurrentDictionary<string, object> setters = new
            ConcurrentDictionary<string, object>();

        public static Action<T, TProperty> GetSetter<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            Ensure.ArgumentNotNull(expression, "expression");
            var propertyExpression = expression.Body as MemberExpression;
            var property = (propertyExpression != null) ? (propertyExpression.Member as PropertyInfo) : null;
            if (property == null)
                throw new ArgumentException("Parameter 'expression' is not a property access expression.",
                    "expression");
            return GetSetter<TProperty>(property);
        }

        public static Action<T, TProperty> GetSetter<TProperty>(string propertyName)
        {
            Ensure.ArgumentNotNullOrEmpty(propertyName, "propertyName");
            var property = typeof(T).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
            if (property == null)
                throw new PropertyNotFoundException<T>(propertyName);
            return GetSetter<TProperty>(property);
        }

        public static Action<T, TProperty> GetSetter<TProperty>(PropertyInfo property)
        {
            Ensure.ArgumentNotNull(property, "property");
            if (property.DeclaringType != typeof(T))
                throw new PropertyNotFoundException<T>(property);
            if (property.GetIndexParameters().Length > 0)
                throw new NotSupportedException("Indexed properties are not supported.");
            return (Action<T, TProperty>)setters.GetOrAdd(property.Name, name =>
            {
                var instanceParameter = Expression.Parameter(typeof(T), "x");
                var valueParameter = Expression.Parameter(typeof(TProperty), "v");
                return
                    Expression.Lambda<Action<T, TProperty>>(
                        Expression.Assign(Expression.Property(instanceParameter, property), valueParameter),
                        instanceParameter, valueParameter
                    )
                    .Compile();
            });
        }
    }
}
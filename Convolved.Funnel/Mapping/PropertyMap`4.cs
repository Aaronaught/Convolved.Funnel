using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Convolved.Funnel.Mapping
{
    public class PropertyMap<T, TProperty, TContext, TData> : IPropertyMap<T, TContext>
        where TContext : FileContext
    {
        private readonly Action<T, TProperty> setter;
        private readonly IField<TContext, TData> field;
        private readonly Func<TData, TProperty> selector;

        public PropertyMap(Expression<Func<T, TProperty>> property, IField<TContext, TData> field,
            Func<TData, TProperty> selector)
        {
            Ensure.ArgumentNotNull(property, "property");
            Ensure.ArgumentNotNull(field, "field");
            Ensure.ArgumentNotNull(selector, "selector");
            this.setter = PropertyMapper<T>.GetSetter(property);
            this.field = field;
            this.selector = selector;
        }

        public Task ExtractAsync(TContext context, T target)
        {
            Ensure.ArgumentNotNull(context, "context");
            return Task.Run(() =>
            {
                var fieldValue = field.ReadValue(context);
                var propertyValue = selector(fieldValue);
                setter(target, propertyValue);
            });
        }
    }
}
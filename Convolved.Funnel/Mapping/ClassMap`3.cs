using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Convolved.Funnel.Mapping
{
    public class ClassMap<T, TContext, TData> : IClassMap<T, TContext>
        where TContext : FileContext
    {
        private readonly List<IPropertyMap<T, TContext>> propertyMaps = new 
            List<IPropertyMap<T, TContext>>();

        public async Task ExtractAsync(TContext context, T target)
        {
            foreach (var propertyMap in propertyMaps)
                await propertyMap.ExtractAsync(context, target);
        }

        public void Property(IPropertyMap<T, TContext> propertyMap)
        {
            Ensure.ArgumentNotNull(propertyMap, "propertyMap");
            propertyMaps.Add(propertyMap);
        }

        public void Property<TProperty>(Expression<Func<T, TProperty>> property, IField<TContext, TData> field,
            Func<TData, TProperty> selector)
        {
            Ensure.ArgumentNotNull(property, "property");
            Ensure.ArgumentNotNull(field, "field");
            Ensure.ArgumentNotNull(selector, "selector");
            propertyMaps.Add(new PropertyMap<T, TProperty, TContext, TData>(property, field, selector));
        }
    }
}
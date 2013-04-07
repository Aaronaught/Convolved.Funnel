using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Convolved.Funnel.Mapping
{
    public class ComponentMap<T, TComponent, TContext> : IPropertyMap<T, TContext>
        where TContext : FileContext
    {
        private readonly Func<TContext, SectionContext<TContext>> sectionContextSelector;
        private readonly Func<TComponent> componentInitializer;
        private readonly Func<SectionContext<TContext>, bool> endOfSectionPredicate;
        private readonly IClassMap<TComponent, TContext> innerMap;
        private readonly Action<T, List<TComponent>> setter;

        public ComponentMap(Expression<Func<T, TComponent>> property,
            Func<TContext, SectionContext<TContext>> sectionContextSelector,
            Func<TComponent> componentInitializer,
            IClassMap<TComponent, TContext> innerMap,
            Func<SectionContext<TContext>, bool> endOfSectionPredicate = null)
            : this(sectionContextSelector, componentInitializer, innerMap, endOfSectionPredicate)
        {
            Ensure.ArgumentNotNull(property, "property");
            var propertySetter = PropertyMapper<T>.GetSetter(property);
            this.setter = (instance, sequence) =>
                propertySetter(instance, sequence.SingleOrDefault());
        }

        public ComponentMap(Expression<Func<T, IEnumerable<TComponent>>> property,
            Func<TContext, SectionContext<TContext>> sectionContextSelector,
            Func<TComponent> componentInitializer,
            IClassMap<TComponent, TContext> innerMap,
            Func<SectionContext<TContext>, bool> endOfSectionPredicate = null)
            : this(sectionContextSelector, componentInitializer, innerMap, endOfSectionPredicate)
        {
            Ensure.ArgumentNotNull(property, "property");
            this.setter = PropertyMapper<T>.GetSetter(property);
        }

        protected ComponentMap(Func<TContext, SectionContext<TContext>> sectionContextSelector,
            Func<TComponent> componentInitializer,
            IClassMap<TComponent, TContext> innerMap,
            Func<SectionContext<TContext>, bool> endOfSectionPredicate = null)
        {
            Ensure.ArgumentNotNull(sectionContextSelector, "sectionContextSelector");
            Ensure.ArgumentNotNull(componentInitializer, "initializer");
            Ensure.ArgumentNotNull(innerMap, "innerMap");
            this.sectionContextSelector = sectionContextSelector;
            this.componentInitializer = componentInitializer;
            this.innerMap = innerMap;
            this.endOfSectionPredicate = endOfSectionPredicate ?? (sc => true);
        }

        public async Task ExtractAsync(TContext context, T target)
        {
            var sectionContext = sectionContextSelector(context);
            var components = await ReadComponents(sectionContext);
            setter(target, components);
        }

        private async Task<List<TComponent>> ReadComponents(SectionContext<TContext> sectionContext)
        {
            var components = new List<TComponent>();
            if (endOfSectionPredicate(sectionContext))
                return components;
            while (true)
            {
                bool hasRecord = await sectionContext.ReadNextRecord();
                if (!hasRecord || endOfSectionPredicate(sectionContext))
                    break;
                var component = componentInitializer();
                await innerMap.ExtractAsync(sectionContext.FileContext, component);
                components.Add(component);
            }
            return components;
        }
    }
}
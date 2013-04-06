using System;
using System.Reflection;

namespace Convolved.Funnel.Mapping
{
    [Serializable]
    public class PropertyNotFoundException<T> : FunnelException
    {
        public PropertyNotFoundException(string propertyName)
            : base(string.Format("Type {0} does not have a public property named '{1}'.", propertyName))
        {
            this.PropertyName = propertyName;
        }

        public PropertyNotFoundException(PropertyInfo property)
            : base(string.Format("Type {0} does not declare property {1}.", 
                property.DeclaringType.FullName + "." + property.Name))
        {
            this.PropertyName = property.Name;
        }

        public string PropertyName { get; private set; }
    }
}
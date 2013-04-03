using System;

namespace Convolved.Funnel.Mapping
{
    public class PropertyNotMappedException : Exception
    {
        public PropertyNotMappedException(Type type, string propertyName)
            : this(type, propertyName, 
                string.Format("No property named '{0}' is mapped on type {1}.", 
                    propertyName, type.FullName))
        {
        }

        public PropertyNotMappedException(Type type, string propertyName, string message) 
            : base(message)
        {
            this.Type = type;
            this.PropertyName = propertyName;
        }

        public Type Type { get; private set; }
        public string PropertyName { get; private set; }
    }
}
using System;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigurationRoot : IFluentInterface
    {
        IConfigure Pickup(string filesearchExpression);
    }
}
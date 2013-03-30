using System;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigurationRoot
    {
        IConfigure Pickup(string filesearchExpression);
    }
}
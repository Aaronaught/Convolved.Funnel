using System;

namespace Convolved.Funnel.Text
{
    public interface IParse<TProperty>
    {
        TProperty Parse(string text);
    }
}
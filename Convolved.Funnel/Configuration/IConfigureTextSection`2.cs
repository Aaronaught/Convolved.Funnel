using System;

namespace Convolved.Funnel.Configuration
{
    public interface IConfigureTextSection<T, TSection> : IConfigureText<T>
    {
        IConfigureTextSection<T, TSection> EndsWhen(Predicate<string> lineCondition);
        IConfigureTextSection<T, TSection> EndsWith(string lineText);
        IConfigureTextSection<T, TSection> EndsWithBlankLine();
        IConfigureTextSectionExactly<T, TSection> Exactly(uint lineCount);
        IConfigureFields<T, TSection, TSection> Fields { get; }
    }
}
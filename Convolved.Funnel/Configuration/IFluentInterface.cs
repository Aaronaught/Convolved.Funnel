using System;
using System.ComponentModel;

namespace Convolved.Funnel.Configuration
{
    /// <summary>
    /// A hack to hide methods defined on <see cref="object"/> for IntelliSense
    /// on fluent interfaces. Credit to Daniel Cazzulino.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentInterface
    {
        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object other);
    }
}
using System;

namespace Convolved.Funnel
{
    /// <summary>
    /// Encapsulates a field for a particular type of file.
    /// </summary>
    /// <typeparam name="TContext">The type of context which indicates the type of file being
    /// read.</typeparam>
    /// <typeparam name="TData">The type of data that is read from the file. This is usually either
    /// text (<see cref="System.String"/>) or binary (<see cref="System.Byte[]"/>).</typeparam>
    public interface IField<TContext, TData>
        where TContext : FileContext
    {
        /// <summary>
        /// Reads data from the specified file.
        /// </summary>
        /// <param name="context">The context representing the file to be read..</param>
        /// <returns>A <see cref="TData"/> instance read from the current file position using this
        /// field definition.</returns>
        TData ReadValue(TContext context);
    }
}
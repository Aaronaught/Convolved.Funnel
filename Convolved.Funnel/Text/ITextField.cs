using System;

namespace Convolved.Funnel.Text
{
    /// <summary>
    /// Encapsulates a field in a text file or section.
    /// </summary>
    public interface ITextField
    {
        /// <summary>
        /// Reads the current field value.
        /// </summary>
        /// <param name="context">The context indicating the current line and position within a file.</param>
        /// <returns>The field value within the specified <paramref name="context"/>.</returns>
        string ReadValue(TextFileContext context);
    }
}
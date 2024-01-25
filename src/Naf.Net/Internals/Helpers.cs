using System;

namespace Naf.Net.Internals;

/// <summary>
/// Helper methods.
/// </summary>
internal static class Helpers
{
    /// <summary>
    /// Assert the value to be required and throws an exception is empty.
    /// </summary>
    /// <param name="value">Object value.</param>
    internal static void Assert(object value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));
    }
    /// <summary>
    /// Assert the value to be required and throws an exception is empty.
    /// </summary>
    /// <param name="value">String of value.</param>
    internal static void Assert(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(value);
    }
}

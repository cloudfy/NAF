using System;

namespace Naf;

public static class Argument
{
    /// <summary>
    /// Defines that the <paramref name="value"/> is required.
    /// </summary>
    /// <param name="value">Nullable string.</param>
    /// <param name="message">Message to pass to the ArgumentNullException.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string NotNull(string? value, string message)
    {
        if (value is null)
            throw new ArgumentNullException(message);

        return value!;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static object NotNull(object? value, string message)
    {
        if (value is null)
            throw new ArgumentNullException(message);

        return value!;
    }
}

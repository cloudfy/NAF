using System;
using System.Text;

namespace Naf.Core;

/// <summary>
/// Represents a string that can be converted to and from a base64 string.
/// </summary>
/// <param name="bytes">Required. Byte-array.</param>
public readonly struct Base64String(
    byte[] bytes)
{
    private readonly byte[] _bytes = bytes;

    public static explicit operator Base64String(string value)
    {
        return new Base64String(Encoding.UTF8.GetBytes(value));
    }
    public static explicit operator string(Base64String value)
    {
        return value.AsString();
    }
    public static Base64String FromBase64(string value)
    {
        return new Base64String(Convert.FromBase64String(value));
    }
    public override string ToString()
    {
        return Convert.ToBase64String(_bytes);
    }
    public string AsString()
    {
        return Encoding.UTF8.GetString(_bytes);
    }
}
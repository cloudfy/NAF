using System;

namespace Naf.Core;

/// <summary>
/// Represents a byte-array that can be converted to and from a base64 string.
/// </summary>
/// <param name="bytes">Required. Byte-array.</param>
public readonly struct Base64Byte(byte[] bytes)
{
    private readonly byte[] _bytes = bytes;

    public static explicit operator Base64Byte(byte[] value)
    {
        return new Base64Byte(value);
    }
    public static explicit operator byte[](Base64Byte value)
    {
        return value.ToByteArray();
    }
    public byte[] ToByteArray() => _bytes;

    public static Base64Byte FromBase64(byte[] value)
    {
        return new Base64Byte(value);
    }
    public static Base64Byte FromBase64(string value)
    {
        return new Base64Byte(Convert.FromBase64String(value));
    }
    public override string ToString()
    {
        return Convert.ToBase64String(_bytes);
    }
}

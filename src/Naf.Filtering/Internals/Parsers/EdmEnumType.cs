using Naf.Filtering.Model;

namespace Naf.Filtering.Internals.Parsers;

/// <summary>
/// Represents an Enum type in the Entity Data Model.
/// </summary>
/// <seealso cref="EdmType" />
[System.Diagnostics.DebuggerDisplay("{Name}: {ClrType}")]
public sealed class EdmEnumType(
    Type clrType
    , IReadOnlyList<EdmEnumMember> members) 
    : EdmType(clrType.Name, clrType.FullName, clrType)
{
    /// <summary>
    /// Gets the enum members.
    /// </summary>
    public IReadOnlyList<EdmEnumMember> Members { get; } = members ?? throw new ArgumentNullException(nameof(members));

    /// <summary>
    /// Gets the CLR Enum value for the specified Enum member in the Entity Data Model.
    /// </summary>
    /// <param name="value">The Enum string value in the Entity Data Model.</param>
    /// <returns>An object containing the CLR Enum value.</returns>
    public object GetClrValue(string value) => Enum.Parse(ClrType, value);
}
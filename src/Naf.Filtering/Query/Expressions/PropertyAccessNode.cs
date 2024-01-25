using Naf.Filtering.Model;
using System.Collections.ObjectModel;

namespace Naf.Filtering.Query.Expressions;

/// <summary>
/// A QueryNode which represents a property.
/// </summary>
[System.Diagnostics.DebuggerDisplay("{Properties}")]
public sealed class PropertyAccessNode : ValueNode
{
    /// <summary>
    /// Initialises a new instance of the <see cref="PropertyAccessNode"/> class.
    /// </summary>
    /// <param name="properties">The property being referenced in the query.</param>
    public PropertyAccessNode(IEnumerable<EdmProperty> properties)
    {
        Properties = new ReadOnlyCollection<EdmProperty>(properties.ToList());
    }

    /// <summary>
    /// Gets the kind of query node.
    /// </summary>
    public override QueryNodeKind Kind => QueryNodeKind.PropertyAccess;

    /// <summary>
    /// Gets the properties being referenced in the query.
    /// </summary>
    public IReadOnlyList<EdmProperty> Properties { get; }

    /// <summary>
    /// Gets the <see cref="EdmType"/> of the property value.
    /// </summary>
    public override EdmType EdmValueType => Properties.Last().PropertyType;
}
using Naf.Filtering.Query.Expressions;

namespace Naf.Filtering.Query;

/// <summary>
/// The base class for a query node.
/// </summary>
public abstract class QueryNode
{
    /// <summary>
    /// Gets the kind of query node.
    /// </summary>
    public abstract QueryNodeKind Kind { get; }
}

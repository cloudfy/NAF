﻿using Naf.Filtering.Model;

namespace Naf.Filtering.Query.Expressions;

/// <summary>
/// A QueryNode which represents a value (property or constant).
/// </summary>
[System.Diagnostics.DebuggerDisplay("{LiteralText}")]
public abstract class ValueNode : QueryNode
{
    /// <summary>
    /// Gets the <see cref="EdmType"/> of the value node.
    /// </summary>
    public abstract EdmType EdmValueType { get; }
}

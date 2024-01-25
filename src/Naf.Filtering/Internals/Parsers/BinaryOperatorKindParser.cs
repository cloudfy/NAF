using Naf.Filtering.Exceptions;
using Naf.Filtering.Query.Expressions;

namespace Naf.Filtering.Internals.Parsers;

internal static class BinaryOperatorKindParser
{
    private static readonly Dictionary<string, BinaryOperatorKind> _operatorTypeMap = new()
    {
        ["add"] = BinaryOperatorKind.Add,
        ["and"] = BinaryOperatorKind.And,
        ["div"] = BinaryOperatorKind.Divide,
        ["eq"] = BinaryOperatorKind.Equal,
        ["ge"] = BinaryOperatorKind.GreaterThanOrEqual,
        ["gt"] = BinaryOperatorKind.GreaterThan,
        ["has"] = BinaryOperatorKind.Has,
        ["le"] = BinaryOperatorKind.LessThanOrEqual,
        ["lt"] = BinaryOperatorKind.LessThan,
        ["mul"] = BinaryOperatorKind.Multiply,
        ["mod"] = BinaryOperatorKind.Modulo,
        ["ne"] = BinaryOperatorKind.NotEqual,
        ["or"] = BinaryOperatorKind.Or,
        ["sub"] = BinaryOperatorKind.Subtract,
        ["in"] = BinaryOperatorKind.In
    };

    internal static BinaryOperatorKind ToBinaryOperatorKind(this string operatorType)
    {
        if (_operatorTypeMap.TryGetValue(operatorType, out var binaryOperatorKind))
        {
            return binaryOperatorKind;
        }

        throw new ParserException(string.Format("UnknownOperator {0}", operatorType));
    }
}

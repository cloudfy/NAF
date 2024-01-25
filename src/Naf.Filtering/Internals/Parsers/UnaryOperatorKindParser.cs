using Naf.Filtering.Exceptions;
using Naf.Filtering.Query.Expressions;

namespace Naf.Filtering.Internals.Parsers;

internal static class UnaryOperatorKindParser
{
    internal static UnaryOperatorKind ToUnaryOperatorKind(this string operatorType)
    {
        switch (operatorType)
        {
            case "not":
                return UnaryOperatorKind.Not;

            default:
                throw new ParserException(string.Format("UnknownOperator {0}", operatorType));
        }
    }
}

namespace Naf.Filtering.Exceptions;

public sealed class InvalidFilterException
    : FilteringException
{
    internal InvalidFilterException(string message)
        : base(message)
    {
    }
}

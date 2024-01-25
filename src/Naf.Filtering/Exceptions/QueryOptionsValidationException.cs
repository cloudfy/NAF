namespace Naf.Filtering.Exceptions;

public class QueryOptionsValidationException : FilteringException
{
    internal QueryOptionsValidationException(string? message) : base(message)
    { }
}

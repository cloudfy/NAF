namespace Naf.Filtering.Exceptions;

public abstract class FilteringException : Exception
{
    internal FilteringException(string? message, Exception? innerException = null) 
        : base(message, innerException)
    {
    }
}

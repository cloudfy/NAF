using System;

namespace Naf.ResultObjects;

public sealed class ResultError
     : IEquatable<ResultError>
{
    public ResultError(string? message, string? code, Exception? exception)
    {
        Exception = exception;
        Message = message;
        Code = code;
    }
    public ResultError(string? message, string? code)
    {
        Message = message;
        Code = code;
    }
    public ResultError(string? message, Exception? exception)
    {
        Message = message;
        Exception = exception;
    }
    public ResultError(string? message)
    {
        Message = message;
    }
    public ResultError(Exception exception)
    {
        Message = exception.Message;
        Exception = exception;
    }
    public string? Message { get; }
    public string? Code { get; }
    /// <summary>
    /// Get the exception associated with this result error.
    /// </summary>
    public Exception? Exception { get; }

    /// <summary>
    /// is this error is associated with an exception or not
    /// </summary>
    public bool HasExceptionError => Exception is not null;

    public bool Equals(ResultError other)
    {
        if (other.HasExceptionError && HasExceptionError)
            return other.Exception!.Equals(Exception);

        if (other.Code is not null && Code is not null)
            return other.Code.Equals(Code);
        if (other.Message is not null && Message is not null)
            return other.Message.Equals(Message);

        return false;
    }
    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (obj is ResultError error) return Equals(error);
        return false;
    }

    public override int GetHashCode()
    {
        if (HasExceptionError)
            return Exception!.GetHashCode();

        unchecked
        {
            var hashCode = 13;
            hashCode = (hashCode * 397) ^ (Code is not null ? Code.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Message is not null ? Message.GetHashCode() : 0);
            return hashCode;
        }
    }
}
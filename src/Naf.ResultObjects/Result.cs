using System.Collections.Generic;

namespace Naf.ResultObjects;

public class Result
{
    private readonly bool _isSuccess;
    private readonly LinkedList<ResultError> _errors;

    internal Result(bool isSuccess) 
    { 
        _isSuccess = isSuccess;
        _errors = new();
    }

    public bool IsSuccess => _isSuccess;
    public string? Message { get; internal set; } = null!;
    public string? LogTrace { get; internal set; } = null!;
    public IReadOnlyCollection<ResultError> Errors 
        => _errors;

    public static Result Success() => new (true);
    public static Result<T> Success<T>(T result) => new(true, result);
    public static Result Failure() => new (false);
    public static Result<T> Failure<T>(T result) => new(false, result);

    internal void AddError(ResultError resultError)
    {
        _errors.AddLast(resultError);
    }
}
namespace Naf.ResultObjects;

public class Result<T> : Result
{
    private readonly T _result;

    internal Result(bool isSuccess, T result)
        : base(isSuccess) => _result = result;

    public T Value => _result;
}
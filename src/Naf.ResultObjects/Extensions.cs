using System;

namespace Naf.ResultObjects;

public static class Extensions
{
    /// <summary>
    /// Adds a <paramref name="message"/> to the result.
    /// </summary>
    /// <typeparam name="T">Type of result object.</typeparam>
    /// <param name="result">Required.</param>
    /// <param name="message">Required. Message to add.</param>
    /// <returns><see cref="Result{T}"/></returns>
    /// <remarks>Any existing message will the replaced.</remarks>
    public static Result<T> WithMessage<T>(this Result<T> result, string message)
    {
        result.Message = message;
        return result;
    }
    /// <summary>
    /// Appends a <see cref="ResultError"/> to the result.
    /// </summary>
    /// <typeparam name="T">Type of result object.</typeparam>
    /// <param name="result">Required.</param>
    /// <param name="message">Required. Message of result error.</param>
    /// <param name="code">Optional. Code of result error.</param>
    /// <returns><see cref="Result{T}"/></returns>
    public static Result<T> WithError<T>(this Result<T> result, string message, string? code = null)
    {
        result.AddError(new ResultError(message, code));
        return result;
    }
    /// <summary>
    /// Appends a <see cref="ResultError"/> to the result.
    /// </summary>
    /// <typeparam name="T">Type of result object.</typeparam>
    /// <param name="result">Required.</param>
    /// <param name="resultError">Required. Result error to append.</param>
    /// <returns><see cref="Result{T}"/></returns>
    public static Result<T> WithError<T>(this Result<T> result, ResultError resultError)
    {
        result.AddError(resultError);
        return result;
    }
    /// <summary>
    /// Appends a <see cref="ResultError"/> of message, code and/or exception to the result.
    /// </summary>
    /// <typeparam name="T">Type of result object.</typeparam>
    /// <param name="result">Required.</param>
    /// <param name="message">Optional. Message of result error.</param>
    /// <param name="exception">Optional. Exception of result error.</param>
    /// <param name="code">Optional. Code of result error.</param>
    /// <returns><see cref="Result{T}"/></returns>
    public static Result<T> WithError<T>(this Result<T> result, string? message, string? code, Exception? exception)
    {
        result.AddError(new ResultError(message, code, exception));
        return result;
    }
    /// <summary>
    /// Appends a <see cref="ResultError"/> to the result of the <paramref name="exception"/>.
    /// </summary>
    /// <typeparam name="T">Type of result object.</typeparam>
    /// <param name="result">Required.</param>
    /// <param name="exception">Optional. Exception of result error.</param>
    /// <returns><see cref="Result{T}"/></returns>
    public static Result<T> WithException<T>(this Result<T> result, Exception exception)
    {
        result.AddError(new ResultError(exception.Message, exception));
        return result;
    }
    /// <summary>
    /// Adds a log trace code to the result.
    /// </summary>
    /// <typeparam name="T">Type of result object.</typeparam>
    /// <param name="result">Required.</param>
    /// <param name="logTrace">Required. Log trace to add.</param>
    /// <returns><see cref="Result{T}"/></returns>
    /// <remarks>Any existing log trace will be replaced.</remarks>
    public static Result<T> WithLogTrace<T>(this Result<T> result, string logTrace)
    {
        result.LogTrace = logTrace;
        return result;
    }
    /// <summary>
    /// Adds a <paramref name="message"/> to the result.
    /// </summary>
    /// <param name="result">Required.</param>
    /// <param name="message">Required. Message to add.</param>
    /// <returns><see cref="Result"/></returns>
    /// <remarks>Any existing message will the replaced.</remarks>
    public static Result WithMessage(this Result result, string message)
    {
        result.Message = message;
        return result;
    }
    /// <summary>
    /// Appends a <see cref="ResultError"/> of <paramref name="message"/> and <paramref name="code"/> to the result.
    /// </summary>
    /// <param name="result">Required.</param>
    /// <param name="message">Optional. Message of result error.</param>
    /// <param name="code">Optional. Code of result error.</param>
    /// <returns><see cref="Result"/></returns>
    public static Result WithError(this Result result, string message, string code)
    {
        result.AddError(new ResultError(message, code));
        return result;
    }
    /// <summary>
    /// Appends a <see cref="ResultError"/> to the result.
    /// </summary>
    /// <param name="result">Required.</param>
    /// <param name="resultError">Required. Result error.</param>
    /// <returns><see cref="Result"/></returns>
    public static Result WithError(this Result result, ResultError resultError)
    {
        result.AddError(resultError);
        return result;
    }
    /// <summary>
    /// Adds a log trace code to the result.
    /// </summary>
    /// <param name="result">Required.</param>
    /// <param name="logTrace">Required. Log trace to add.</param>
    /// <returns><see cref="Result"/></returns>
    /// <remarks>Any existing log trace will be replaced.</remarks>
    public static Result WithLogTrace(this Result result, string logTrace)
    {
        result.LogTrace = logTrace;
        return result;
    }
    /// <summary>
    /// Appends a <see cref="ResultError"/> of <paramref name="message"/>, <paramref name="code"/> and/or <paramref name="exception"/> to the result.
    /// </summary>
    /// <param name="result">Required.</param>
    /// <param name="message">Optional. Message of result error.</param>
    /// <param name="exception">Optional. Exception of result error.</param>
    /// <param name="code">Optional. Code of result error.</param>
    /// <returns><see cref="Result"/></returns>
    public static Result WithError(this Result result, string? message, string? code, Exception? exception)
    {
        result.AddError(new ResultError(message, code, exception));
        return result;
    }
    /// <summary>
    /// Appends a <see cref="ResultError"/> to the result of the <paramref name="exception"/>.
    /// </summary>
    /// <param name="result">Required.</param>
    /// <param name="exception">Optional. Exception of result error.</param>
    /// <returns><see cref="Result"/></returns>
    public static Result WithException(this Result result, Exception exception)
    {
        result.AddError(new ResultError(exception.Message, exception));
        return result;
    }

    /// <summary>
    /// Returns a Failure result with the <paramref name="exception"/> as the error.
    /// </summary>
    /// <param name="exception">Required. Exception to add.</param>
    /// <returns><see cref="Result"/></returns>
    public static Result AsResult(this Exception exception)
    {
        return Result.Failure().WithException(exception);
    }
    /// <summary>
    /// Returns a Failure result with the <paramref name="exception"/> as the error, and <paramref name="result"/>.
    /// </summary>
    /// <typeparam name="T">Type of result object.</typeparam>
    /// <param name="exception">Required. Exception to add.</param>
    /// <param name="result">Required. Result to add as value.</param>
    /// <returns><see cref="Result{T}"/></returns>
    public static Result<T> AsResult<T>(this Exception exception, T result)
    {
        return Result.Failure(result).WithException(exception);
    }
}
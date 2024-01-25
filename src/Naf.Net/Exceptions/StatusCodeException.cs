using System;
using System.Net;
using System.Net.Http;

namespace Naf.Net.Exceptions;

public class StatusCodeException : Exception
{
    public StatusCodeException(
        HttpStatusCode statusCode, HttpRequestMessage requestMessage, HttpResponseMessage responseMessage)
    {
        StatusCode = statusCode;
        RequestMessage = requestMessage;
        ResponseMessage = responseMessage;
    }

    public HttpStatusCode StatusCode { get; }
    public HttpRequestMessage RequestMessage { get; }
    public HttpResponseMessage ResponseMessage { get; }
}

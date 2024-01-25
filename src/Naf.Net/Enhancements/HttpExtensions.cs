using Naf.Net.Exceptions;
using System.Net.Http;

namespace Naf.Net.Enhancements;

public static class HttpExtensions 
{ 
    public static void EnsureSuccess(this HttpResponseMessage responseMessage)
    {
        if ((int)responseMessage.StatusCode >= 200 &&
            (int)responseMessage.StatusCode <= 299)
            return;

        throw new StatusCodeException(
            responseMessage.StatusCode, responseMessage.RequestMessage, responseMessage);
    }
}

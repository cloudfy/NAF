using Newtonsoft.Json;
using Naf.Net.Internals;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Naf.Net.Json;

/// <summary>
/// Provides methods for communicating with Json endpoints.
/// </summary>
public sealed class JsonClient(HttpClient httpClient)
{
    private readonly HttpClient _innerHttpClient = httpClient;

    public JsonClient(IHttpClientFactory httpClientFactory, string? name = null)
        : this(httpClientFactory.CreateClient(name))
    {
    }

    #region === static methods ===
    /// <summary>Converts a string into a base64 string.</summary>
    /// <param name="input">String to convert.</param>
    /// <returns>string</returns>
    public static string ToBase64(string input)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
    }
    /// <summary>
    /// Converts an object to a base64 string.
    /// </summary>
    /// <typeparam name="T">Type to convert to.</typeparam>
    /// <param name="input">Object to serialize.</param>
    /// <returns></returns>
    public static string ToBase64<T>(T input)
    {
        string jsonValue = JsonConvert.SerializeObject(input);
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonValue));
    }
    /// <summary>Convert a Base64 string into a string.</summary>
    /// <param name="output">Base64 string to convert.</param>
    /// <returns></returns>
    public static string FromBase64(string output)
    {
        var bytes = Convert.FromBase64String(output);
        return Encoding.UTF8.GetString(bytes);
    }
    /// <summary>Converts a Base64 string into an object.</summary>
    /// <typeparam name="T">Type to return.</typeparam>
    /// <param name="output">Base64 string to convert.</param>
    /// <returns></returns>
    public static T FromBase64<T>(string output)
    {
        if (string.IsNullOrEmpty(output))
            throw new ArgumentNullException("output");

        var bytes = Convert.FromBase64String(output);
        string outputString = Encoding.UTF8.GetString(bytes);
        return JsonConvert.DeserializeObject<T>(outputString);
    }
    #endregion

    #region === PUT ===
    /// <summary>Provides a serialized PUT method for an API.</summary>
    /// <param name="url">Url of the request.</param>
    /// <param name="request">Request of the method.</param>
    /// <param name="headers">HTTP headers of the request.</param>
    /// <param name="jsonSerializerSettings"></param>
    /// <returns>TResponse</returns>
    public async Task<TResponse> Put<TRequest, TResponse>(
        string url
        , TRequest request
        , StringDictionary headers
        , JsonSerializerSettings jsonSerializerSettings = null)
    {
        return await ExecuteRequest<TRequest, TResponse>(url, HttpMethodEnum.Put, request, headers, jsonSerializerSettings);
    }
    #endregion

    #region === POST ===
    /// <summary>Provides a serialized POST method for an API.</summary>
    /// <param name="url">Url of the request.</param>
    /// <param name="request">Body of the request.</param>
    /// <param name="headers">HTTP headers of the request.</param>
    /// <returns>TResponse</returns>
    public async Task<TResponse> Post<TRequest, TResponse>(string url, TRequest request, StringDictionary headers)
    {
        return await ExecuteRequest<TRequest, TResponse>(url, HttpMethodEnum.Post, request, headers, null);
    }
    #endregion

    #region === GET ===
    /// <summary></summary>
    /// <param name="url"></param>
    /// <param name="headers"></param>
    /// <param name="jsonSerializerSettings"></param>
    /// <returns>TResponse</returns>
    public async Task<TResponse> Get<TResponse>(
        string url
        , StringDictionary headers
        , JsonSerializerSettings jsonSerializerSettings = null)
    {
        return await ExecuteRequest<object, TResponse>(url, HttpMethodEnum.Get, null, headers, jsonSerializerSettings);
    }
    #endregion

    #region === DEL ===
    /// <summary></summary>
    /// <param name="url"></param>
    /// <param name="headers"></param>
    /// <returns>bool</returns>
    public async Task<bool> Delete(string url, StringDictionary headers)
    {
        await ExecuteRequest(url, HttpMethodEnum.Delete, headers, null);
        return true;
    }
    #endregion

    #region === PATCH ===
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="url"></param>
    /// <param name="request"></param>
    /// <param name="headers"></param>
    /// <returns></returns>
    public async Task<TResponse> Patch<TResponse>(string url, Dictionary<string, object> request, StringDictionary headers)
    {
        return await ExecuteRequest<Dictionary<string, object>, TResponse>(
            url, HttpMethodEnum.Patch, request, headers, null);
    }
    #endregion

    #region === private methods ===
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResponse">Type of response.</typeparam>
    /// <typeparam name="TRequest">Type of request.</typeparam>
    /// <param name="url"></param>
    /// <param name="method"></param>
    /// <param name="request"></param>
    /// <param name="headers"></param>
    /// <returns></returns>
    private async Task<TResponse> ExecuteRequest<TRequest, TResponse>(
        string url, HttpMethodEnum method
        , TRequest request
        , StringDictionary headers, JsonSerializerSettings jsonSerializerSettings)
    {
        var requestMessage = new HttpRequestMessage(GetHttpMethod(method), url);
        if (request != null)
            requestMessage.Content = new JsonContent(request);

        foreach (string key in headers.Keys)
            requestMessage.Headers.Add(key, headers[key]);

        try
        {
            var response = await this._innerHttpClient.SendAsync(requestMessage);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(responseString);

        }
        catch (Exception)
        {
            throw;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <param name="method"></param>
    /// <param name="headers"></param>
    /// <param name="jsonSerializerSettings"></param>
    /// <returns></returns>
    private async Task ExecuteRequest(
        string url, HttpMethodEnum method, StringDictionary headers, JsonSerializerSettings jsonSerializerSettings)
    {
        var requestMessage = new HttpRequestMessage(GetHttpMethod(method), url);

        foreach (string key in headers.Keys)
            requestMessage.Headers.Add(key, headers[key]);

        try
        {
            var response = await this._innerHttpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception)
        {
            throw;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static HttpMethod GetHttpMethod(HttpMethodEnum value)
    {
        if (value == HttpMethodEnum.Get)
            return HttpMethod.Get;
        if (value == HttpMethodEnum.Post)
            return HttpMethod.Post;
        if (value == HttpMethodEnum.Put)
            return HttpMethod.Put;
        if (value == HttpMethodEnum.Delete)
            return HttpMethod.Delete;
        if (value == HttpMethodEnum.Patch)
            return new HttpMethod("PATCH");

        throw new NotImplementedException();

    }
    /// <summary>
    /// Returns common serialization settings.
    /// </summary>
    /// <returns></returns>
    private JsonSerializerSettings GetJsonSerializerSettings()
    {
        return new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
    }
    #endregion
}

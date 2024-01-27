using Naf.Net.Tokens;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

namespace Naf.Net.OAuth;

/// <summary>
/// 
/// </summary>
/// <param name="clientId">Required.</param>
/// <param name="defaultRedirectUrl">Optional.</param>
/// <param name="clientSecret">Optional.</param>
/// <param name="httpClientFactory">Optional.</param>
public sealed class OAuthManager(
    string clientId
    , string? defaultRedirectUrl = null
    , string? clientSecret = null
    , IHttpClientFactory? httpClientFactory = null)
{
    private readonly string _clientId = clientId;
    private readonly string? _defaultRedirectUrl = defaultRedirectUrl;
    private readonly string? _clientSecret = clientSecret;
    private readonly IHttpClientFactory? _httpClientFactory = httpClientFactory;

    /// <summary>
    /// Creates an authorization link to initiate the authorization flow.
    /// </summary>
    /// <param name="authorizationEndpoint">Required. Url of the authorization endpoint.</param>
    /// <param name="scopes">Required. </param>
    /// <param name="responseMode">Required.</param>
    /// <param name="responseType">Required.</param>
    /// <param name="redirectUri">Optional.</param>
    /// <param name="state">Optional.</param>
    /// <param name="prompt">Optional.</param>
    /// <param name="nonce">Optional. Nonce of request.</param>
    /// <param name="loginHint">Optional.You can use this parameter to pre-fill the username and email address field of the sign-in page for the user. Apps can use this parameter during reauthentication, after already extracting the login_hint optional claim from an earlier sign-in.</param>
    /// <param name="domainHint">Optional. If included, the app skips the email-based discovery process that user goes through on the sign-in page, leading to a slightly more streamlined user experience. For example, sending them to their federated identity provider. Apps can use this parameter during reauthentication, by extracting the tid from a previous sign-in.</param>
    /// <returns><see cref="Uri"/></returns>
    public Uri CreateAuthorizationLink(
        string authorizationEndpoint
        , string[] scopes
        , ResponseModeEnum responseMode = ResponseModeEnum.query
        , ResponseTypeEnum responseType = ResponseTypeEnum.code
        , string? redirectUri = null
        , string? state = null
        , PromptEnum? prompt = null
        , string? nonce = null
        , string? loginHint = null
        , string? domainHint = null)
    {
        var url = $"{authorizationEndpoint}?client_id={_clientId}";
        url += $"&redirect_uri={redirectUri ?? _defaultRedirectUrl}";
        if (prompt is not null)
            url += $"&prompt={prompt}";
        if (state is not null)
            url += $"&state={state}";
        if (nonce is not null)
            url += $"&nonce={nonce}";
        if (loginHint is not null)
            url += $"&login_hint={loginHint}";
        if (domainHint is not null)
            url += $"&domain_hint={domainHint}";
        url += $"&response_type={responseType}";
        url += $"&response_mode={responseMode}";
        return new Uri(url + $"&scope={UrlEncode(string.Join("+", scopes))}");
    }
    /// <summary>
    /// Request a token from the token authorization endpoint <paramref name="url"/>.
    /// </summary>
    /// <param name="url">Required.</param>
    /// <param name="code">Required.</param>
    /// <param name="scopes">Required.</param>
    /// <param name="redirectUrl">Optional.</param>
    /// <param name="cancellationToken">Optional. Propogates notification that operations should be cancelled.</param>
    /// <returns><see cref="OAuth2Token"/></returns>
    public async Task<OAuth2Token> RequestToken(
        string url
        , string code
        , string[] scopes
        , string? redirectUrl = null
        , CancellationToken cancellationToken = default)
    {
        Dictionary<string, string> requestBody = new()
        {
            { "client_id", _clientId },
            { "client_secret", _clientSecret! },
            { "redirect_uri", GetRedirectUrl(redirectUrl) },
            { "code", code },
            { "scope", string.Join(" ", scopes) },
            { "grant_type", "authorization_code" }
        };

        HttpClient httpClient = _httpClientFactory?.CreateClient("oauth") ?? new();
        HttpRequestMessage request = new(HttpMethod.Post, url)
        {
            Content = new FormUrlEncodedContent(requestBody)
        };

        var response = await httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var contentBody = await response.Content.ReadAsStringAsync();
        return Newtonsoft.Json.JsonConvert.DeserializeObject<OAuth2Token>(contentBody)!;
    }

    /// <summary>
    /// Requests a refresh token from the token authorization endpoint <paramref name="url"/>.
    /// </summary>
    /// <param name="url">Required. Token endpoint for authorization requests.</param>
    /// <param name="token">Required. <see cref="OAuth2Token"/> token.</param>
    /// <param name="scopes">Required. Scopes to refresh token for. Must be the same as the initial request.</param>
    /// <param name="redirectUrl">Required. Redirect url as the initial request.</param>
    /// <param name="cancellationToken">Optional. Propogates notification that operations should be cancelled.</param>
    /// <returns><see cref="OAuth2Token"/></returns>
    public async Task<OAuth2Token> RefreshToken(
        string url
        , OAuth2Token token
        , string[] scopes
        , string? redirectUrl = null
        , CancellationToken cancellationToken = default)
        => await RefreshToken(url, token.RefreshToken!, scopes, redirectUrl, cancellationToken);

    /// <summary>
    /// Requests a refresh token from the token authorization endpoint.
    /// </summary>
    /// <param name="url">Required. Token endpoint for authorization requests.</param>
    /// <param name="refreshToken">Required. Refresh token.</param>
    /// <param name="scopes">Required. Scopes to refresh token for. Must be the same as the initial request.</param>
    /// <param name="redirectUrl">Required. Redirect url as the initial request.</param>
    /// <param name="cancellationToken">Optional. Propogates notification that operations should be cancelled.</param>
    /// <returns><see cref="OAuth2Token"/></returns>
    public async Task<OAuth2Token> RefreshToken(
        string url
        , string refreshToken
        , string[] scopes
        , string? redirectUrl = null
        , CancellationToken cancellationToken = default)
    {
        Dictionary<string, string> requestBody = new()
        {
            { "client_id", _clientId },
            { "client_secret", _clientSecret! },
            { "redirect_uri", GetRedirectUrl(redirectUrl, false) },
            { "refresh_token", refreshToken },
            { "scope", string.Join(" ", scopes) },
            { "grant_type", "refresh_token" }
        };

        HttpClient httpClient = _httpClientFactory?.CreateClient("oauth") ?? new();
        HttpRequestMessage request = new(HttpMethod.Post, url)
        {
            Content = new FormUrlEncodedContent(requestBody)
        };

        var response = await httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var contentBody = await response.Content.ReadAsStringAsync();
        return Newtonsoft.Json.JsonConvert.DeserializeObject<OAuth2Token>(contentBody)!;
    }
    
    private string GetRedirectUrl(string? redirectUrl, bool required = true)
    {
        if (string.IsNullOrEmpty(redirectUrl ?? _defaultRedirectUrl) && required)
            throw new ArgumentException("Redirect url is required");

        return (redirectUrl ?? _defaultRedirectUrl)!;
    }
    /// <summary>
    /// Encodes a URL string.
    /// </summary>
    /// <param name="value">Required. Url to encode.</param>
    /// <returns></returns>
    private static string UrlEncode(string value)
        => System.Web.HttpUtility.UrlEncode(value);
}

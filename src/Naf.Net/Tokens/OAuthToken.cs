using Newtonsoft.Json;

namespace Naf.Net.Tokens;

public class OAuth2Token(
    string accessToken
        , string? refreshToken = null
        , int? expiresIn = null
        , string? tokenType = null
        , string? scope = null) : IAuthToken
{
    [JsonProperty("refresh_token")]
    public string? RefreshToken { get; set; } = refreshToken;
    [JsonProperty("access_token")]
    public string AccessToken { get; set; } = accessToken;
    [JsonProperty("expires_in")]
    public int? ExpiresIn { get; set; } = expiresIn;
    [JsonProperty("token_type")]
    public string? TokenType { get; set; } = tokenType;
    public string? Scope { get; set; } = scope;
}

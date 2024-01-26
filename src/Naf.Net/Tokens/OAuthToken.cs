using Newtonsoft.Json;

namespace Naf.Net.Tokens;

public class OAuth2Token : IAuthToken
{
    [JsonProperty("refresh_token")]
    public string? RefreshToken { get; set; }
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
    [JsonProperty("expires_in")]
    public int? ExpiresIn { get; set; }
    [JsonProperty("token_type")]
    public string? TokenType { get; set; }
    public string? Scope { get; set; }
}

using Newtonsoft.Json;

namespace API.BLL.UseCases.DrkServerConnector.Entities
{
    public class ConnectorToken
    {
        [JsonProperty("access_token")] public string AccessToken { get; set; }
        [JsonProperty("token_type")] public string TokenType { get; set; }
        [JsonProperty("expires_in")] public int ExpiresIn { get; set; }
        [JsonProperty("refresh_token")] public string RefreshToken { get; set; }
        [JsonProperty("refresh_expires_in")] public string RefreshTokenExpiresIn { get; set; }
        [JsonProperty("not-before-policy")] public string NotBeforePolicy { get; set; }
        [JsonProperty("session_state")] public string SessionState { get; set; }
        [JsonProperty("scope")] public string Scope { get; set; }
    }
}
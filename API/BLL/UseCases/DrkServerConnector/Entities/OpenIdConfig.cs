using System.Collections.Generic;
using Newtonsoft.Json;

namespace API.BLL.UseCases.DrkServerConnector.Entities
{
    public class OpenIdConfig
    {
        public OpenIdConfig(string issuer,
        string authorizationEndpoint,
        string tokenEndpoint,
        string introspectionEndpoint,
        string userinfoEndpoint,
        string endSessionEndpoint,
        string jwksUri,
        string checkSessionIframe,
        List<string> grantTypesSupported,
        List<string> responseTypesSupported,
        List<string> subjectTypesSupported,
        List<string> idTokenSigningAlgValuesSupported,
        List<string> idTokenEncryptionAlgValuesSupported,
        List<string> idTokenEncryptionEncValuesSupported,
        List<string> userinfoSigningAlgValuesSupported,
        List<string> requestObjectSigningAlgValuesSupported,
        List<string> responseModesSupported,
        string registrationEndpoint,
        List<string> tokenEndpointAuthMethodsSupported,
        List<string> tokenEndpointAuthSigningAlgValuesSupported,
        List<string> claimsSupported,
        List<string> claimTypesSupported,
        bool claimsParameterSupported,
        List<string> scopesSupported,
        bool requestParameterSupported,
        bool requestUriParameterSupported,
        List<string> codeChallengeMethodsSupported,
        bool tlsClientCertificateBoundAccessTokens)
    {
        Issuer = issuer;
        AuthorizationEndpoint = authorizationEndpoint;
        TokenEndpoint = tokenEndpoint;
        IntrospectionEndpoint = introspectionEndpoint;
        UserinfoEndpoint = userinfoEndpoint;
        EndSessionEndpoint = endSessionEndpoint;
        JwksUri = jwksUri;
        CheckSessionIframe = checkSessionIframe;
        GrantTypesSupported = grantTypesSupported;
        ResponseTypesSupported = responseTypesSupported;
        SubjectTypesSupported = subjectTypesSupported;
        IdTokenSigningAlgValuesSupported = idTokenSigningAlgValuesSupported;
        IdTokenEncryptionAlgValuesSupported = idTokenEncryptionAlgValuesSupported;
        IdTokenEncryptionEncValuesSupported = idTokenEncryptionEncValuesSupported;
        UserinfoSigningAlgValuesSupported = userinfoSigningAlgValuesSupported;
        RequestObjectSigningAlgValuesSupported = requestObjectSigningAlgValuesSupported;
        ResponseModesSupported = responseModesSupported;
        RegistrationEndpoint = registrationEndpoint;
        TokenEndpointAuthMethodsSupported = tokenEndpointAuthMethodsSupported;
        TokenEndpointAuthSigningAlgValuesSupported = tokenEndpointAuthSigningAlgValuesSupported;
        ClaimsSupported = claimsSupported;
        ClaimTypesSupported = claimTypesSupported;
        ClaimsParameterSupported = claimsParameterSupported;
        ScopesSupported = scopesSupported;
        RequestParameterSupported = requestParameterSupported;
        RequestUriParameterSupported = requestUriParameterSupported;
        CodeChallengeMethodsSupported = codeChallengeMethodsSupported;
        TlsClientCertificateBoundAccessTokens = tlsClientCertificateBoundAccessTokens;
    }

    [JsonProperty("issuer")] public string Issuer { get; set; }

    [JsonProperty("authorization_endpoint")]
    public string AuthorizationEndpoint { get; set; }

    [JsonProperty("token_endpoint")] public string TokenEndpoint { get; set; }

    [JsonProperty("introspection_endpoint")]
    public string IntrospectionEndpoint { get; set; }

    [JsonProperty("userinfo_endpoint")] public string UserinfoEndpoint { get; set; }
    [JsonProperty("end_session_endpoint")] public string EndSessionEndpoint { get; set; }
    [JsonProperty("jwks_uri")] public string JwksUri { get; set; }
    [JsonProperty("check_session_iframe")] public string CheckSessionIframe { get; set; }

    [JsonProperty("grant_types_supported")]
    public List<string> GrantTypesSupported { get; set; }

    [JsonProperty("response_types_supported")]
    public List<string> ResponseTypesSupported { get; set; }

    [JsonProperty("subject_types_supported")]
    public List<string> SubjectTypesSupported { get; set; }

    [JsonProperty("id_token_signing_alg_values_supported")]
    public List<string> IdTokenSigningAlgValuesSupported { get; set; }

    [JsonProperty("id_token_encryption_alg_values_supported")]
    public List<string> IdTokenEncryptionAlgValuesSupported { get; set; }

    [JsonProperty("id_token_encryption_enc_values_supported")]
    public List<string> IdTokenEncryptionEncValuesSupported { get; set; }

    [JsonProperty("userinfo_signing_alg_values_supported")]
    public List<string> UserinfoSigningAlgValuesSupported { get; set; }

    [JsonProperty("request_object_signing_alg_values_supported")]
    public List<string> RequestObjectSigningAlgValuesSupported { get; set; }

    [JsonProperty("response_modes_supported")]
    public List<string> ResponseModesSupported { get; set; }

    [JsonProperty("registration_endpoint")]
    public string RegistrationEndpoint { get; set; }

    [JsonProperty("token_endpoint_auth_methods_supported")]
    public List<string> TokenEndpointAuthMethodsSupported { get; set; }

    [JsonProperty("token_endpoint_auth_signing_alg_values_supported")]
    public List<string> TokenEndpointAuthSigningAlgValuesSupported { get; set; }

    [JsonProperty("claims_supported")] public List<string> ClaimsSupported { get; set; }

    [JsonProperty("claim_types_supported")]
    public List<string> ClaimTypesSupported { get; set; }

    [JsonProperty("claims_parameter_supported")]
    public bool ClaimsParameterSupported { get; set; }

    [JsonProperty("scopes_supported")] public List<string> ScopesSupported { get; set; }

    [JsonProperty("request_parameter_supported")]
    public bool RequestParameterSupported { get; set; }

    [JsonProperty("request_uri_parameter_supported")]
    public bool RequestUriParameterSupported { get; set; }

    [JsonProperty("code_challenge_methods_supported")]
    public List<string> CodeChallengeMethodsSupported { get; set; }

    [JsonProperty("tls_client_certificate_bound_access_tokens")]
    public bool TlsClientCertificateBoundAccessTokens { get; set; }
    }
}
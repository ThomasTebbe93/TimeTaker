using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using API.BLL.Base;
using API.BLL.UseCases.DrkServerConnector.Entities;
using Newtonsoft.Json;

namespace API.BLL.UseCases.DrkServerConnector.Services
{
    public class ServerConnector
    {
        private readonly string Scope = "mv.servicelog admin.codeentry";
        private OpenIdConfig Config { get; set; }
        private Dictionary<string, string> TokenForm { get; set; }

        public ServerConnector(string issuer, string clientId, string clientSecret, string username, string password)
        {
            Config = GetConfig(issuer);
            TokenForm = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "username", username },
                { "password", password },
                { "audience", issuer },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "scope", Scope },
            };
        }

        public async Task<EntityOrError<T>> GetFromServer<T>(string endPoint, string body)
        {
            var result = await GetToken();
            if (result.HasError())
                return new EntityOrError<T>()
                {
                    Exception = result.Exception
                };

            try
            {
                var client = new HttpClient();
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                using var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(endPoint),
                    Method = HttpMethod.Post,
                    Content = content
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.Value.AccessToken);
                request.Headers.Add("X-Client-Identification", result.Value.SessionState);
                request.Headers.Add("X-Client-Name", "ServerConnector");
                var response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode();
                var queryResult = await response.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<T>(queryResult);
                if (res == null) throw new Exception("Error requesting Data");
                return new EntityOrError<T>()
                {
                    Value = res
                };
            }
            catch (Exception e)
            {
                return new EntityOrError<T>()
                {
                    Exception = e
                };
            }
        }

        private OpenIdConfig GetConfig(string issuer)
        {
            try
            {
                var discoveryClient = new HttpClient();
                var response = discoveryClient.GetAsync(issuer + "/.well-known/openid-configuration").Result;

                response.EnsureSuccessStatusCode();

                var responseString = response.Content.ReadAsStringAsync().Result;
                var config = JsonConvert.DeserializeObject<OpenIdConfig>(responseString);

                if (config == null) throw new Exception("Error initializing the ServerConnector");
                return config;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Error initializing the ServerConnector");
            }
        }

        private async Task<EntityOrError<ConnectorToken>> GetToken()
        {
            try
            {
                var client = new HttpClient();
                using var request = new HttpRequestMessage(HttpMethod.Post, Config.TokenEndpoint);
                request.Content = new FormUrlEncodedContent(TokenForm);
                var response = await client.SendAsync(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new UnauthorizedAccessException();
                    case HttpStatusCode.OK:
                        break;
                    default:
                        throw new Exception("Error Fetching Data");
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<ConnectorToken>(jsonContent);
                if (token == null) throw new Exception("Error requesting token");
                return new EntityOrError<ConnectorToken>()
                {
                    Value = token
                };
            }
            catch (Exception e)
            {
                return new EntityOrError<ConnectorToken>()
                {
                    Exception = e
                };
            }
        }
    }
}
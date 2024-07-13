using Microsoft.Extensions.Configuration;
using Pinboard.DataPersistence.Models;
using Pinboard.Domain.Interfaces;
using RestSharp;
using System.Web;

namespace Pinboard.DataPersistence
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;

        public AuthService(IConfiguration config) 
        { 
            _config = config;
        }

        public void DeleteAccount(string userId)
        {
            var token = getToken();

            var client = new RestClient($"{_config["Auth0ManagementApi:Audience"]}users");
            var request = new RestRequest(HttpUtility.UrlEncode(userId), Method.Delete);
            request.AddHeader("Authorization", $"Bearer {token}");

            var response = client.Execute(request);
            
            if (!response.IsSuccessful)
            {
                throw new Exception("User deletion was not successful");
            }
        }

        private string? getToken()
        {
            var client = new RestClient($"{_config["Auth0:Domain"]}oauth/token");
            var request = new RestRequest(string.Empty, Method.Post);
            request.AddHeader("content-type", "application/json");
            request.AddParameter(
                "application/json",
                new
                {
                    client_id = _config["Auth0ManagementApi:ClientId"],
                    client_secret = _config["Auth0ManagementApi:ClientSecret"],
                    audience = _config["Auth0ManagementApi:Audience"],
                    grant_type = "client_credentials"
                },
                ParameterType.RequestBody); 
            
            var response = client.Execute<AccessTokenResponse>(request);

            return response?.Data?.access_token;
        }
    }
}

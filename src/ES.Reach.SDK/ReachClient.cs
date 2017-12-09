using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ES.Reach.SDK
{
    public class ReachClient
    {
        public AuthToken AuthToken;
        public GlobalContext GlobalContext;
        private readonly string _clientId, _clientSecret, _serviceUrl = "https://reachsosapis.azurewebsites.net";

        public ReachClient(string clientId, string clientSecret)
        {
            if (string.IsNullOrWhiteSpace(clientId))
                throw new ArgumentNullException("clientId");

            if (string.IsNullOrWhiteSpace(clientSecret))
                throw new ArgumentNullException("clientSecret");

            _clientId = clientId;
            _clientSecret = clientSecret;

            GlobalContext = new GlobalContext(this);
        }

        private async Task AuthorizeAppAsync()
        {
            var httpClient = new HttpClient();

            var tokenRequest = new AuthTokenRequest();
            tokenRequest.Grant_Type = "client_credentials";
            tokenRequest.Client_Id = _clientId;
            tokenRequest.Client_Secret = _clientSecret;

            var response = await httpClient.PostAsync(string.Format("{0}/api/token", _serviceUrl),
                new StringContent(JsonConvert.SerializeObject(tokenRequest), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                AuthToken = JsonConvert.DeserializeObject<AuthToken>(content);
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new ReachClientException { ErrorCode = ReachExceptionCodes.Forbidden };
            else
                throw new ReachClientException { ErrorCode = ReachExceptionCodes.Unknown };
        }

        internal async Task<bool> CheckAuthorization()
        {
            if (null == AuthToken || AuthToken.IsExpired())
                await AuthorizeAppAsync();
            return true;
        }
    }
}
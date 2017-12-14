using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ES.Reach.SDK
{
    public class ReachClient
    {
        public AuthToken AuthToken;
        public GlobalContext GlobalContext;
        internal readonly string _clientId, _clientSecret, _lang, _serviceUrl = "https://reachsosapis.azurewebsites.net";

        public ReachClient(string clientId, string clientSecret, string lang = "en")
        {
            if (string.IsNullOrWhiteSpace(clientId))
                throw new ArgumentNullException("clientId");

            if (string.IsNullOrWhiteSpace(clientSecret))
                throw new ArgumentNullException("clientSecret");

            _clientId = clientId;
            _clientSecret = clientSecret;
            _lang = lang;

            GlobalContext = new GlobalContext(this);
        }

        internal HttpClient CreateHttpClient(double timeoutSeconds = 12)
        {
            var httpClient = new HttpClient(new HttpClientHandler { MaxRequestContentBufferSize = 67108864 });
            httpClient.MaxResponseContentBufferSize = 67108864;
            httpClient.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrWhiteSpace(_lang))
                httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(_lang));

            if (!string.IsNullOrWhiteSpace(AuthToken.Access_Token))
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken.Access_Token);
            return httpClient;
        }

        internal async Task<ReachClientException> ProcessUnsuccessResponseMessage(HttpResponseMessage response)
        {
            try
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    if (null != response.Content)
                    {
                        var apiException = JsonConvert.DeserializeObject<ReachApiException>(await response.Content.ReadAsStringAsync());
                        if (null != apiException)
                        {
                            if (apiException.AppExceptionCode == "Auth_TokenExpired")
                                return new ReachClientException { ErrorCode = ReachExceptionCodes.AuthTokenExpired };
                        }
                    }

                    return new ReachClientException { ErrorCode = ReachExceptionCodes.Unauthorized };
                }
                else
                    return new ReachClientException { ErrorCode = ReachExceptionCodes.ServerUnknown };
            }
            catch (Exception)
            {
                throw new ReachClientException { ErrorCode = ReachExceptionCodes.ClientUnknown };
            }
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
                throw new ReachClientException { ErrorCode = ReachExceptionCodes.ServerUnknown };
        }

        internal async Task<bool> CheckAuthorization(bool force)
        {
            if (null == AuthToken || AuthToken.IsExpiring() || force)
                await AuthorizeAppAsync();
            return true;
        }
    }
}
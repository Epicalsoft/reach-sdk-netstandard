using Epicalsoft.Reach.Api.Client.Net.Contexts;
using Epicalsoft.Reach.Api.Client.Net.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Epicalsoft.Reach.Api.Client.Net
{
    public class ReachClient
    {
        public string Endpoint = "https://reachsosapis.azurewebsites.net";
        public AuthToken AuthToken { get; private set; }
        public GlobalContext GlobalContext;
        public UserContext UserContext;
        public AcceptLanguage Lang { get; }
        private HttpClient _httpClient;
        public HttpClient HttpClient { get { if (null == _httpClient) CreateHttpClient(); return _httpClient; } }
        private readonly string _clientId, _clientSecret, _userkey, _grant_type;

        public ReachClient(string clientId, string clientSecret, AcceptLanguage lang = AcceptLanguage.English)
        {
            if (string.IsNullOrWhiteSpace(clientId))
                throw new ArgumentNullException("clientId");

            if (string.IsNullOrWhiteSpace(clientSecret))
                throw new ArgumentNullException("clientSecret");

            _grant_type = "client_credentials";
            _clientId = clientId;
            _clientSecret = clientSecret;
            Lang = lang;

            GlobalContext = new GlobalContext(this);
        }

        public ReachClient(string userkey, AcceptLanguage lang = AcceptLanguage.English)
        {
            if (string.IsNullOrWhiteSpace(userkey))
                throw new ArgumentNullException("userkey");

            _grant_type = "user_key";
            _userkey = userkey;
            Lang = lang;

            GlobalContext = new GlobalContext(this);
            UserContext = new UserContext(this);
        }

        private string GetLangDescription(AcceptLanguage lang)
        {
            switch (lang)
            {
                case AcceptLanguage.English:
                    return "en";

                case AcceptLanguage.Spanish:
                    return "es";

                default:
                    return "en";
            }
        }

        internal void CreateHttpClient()
        {
            _httpClient = new HttpClient(new HttpClientHandler { MaxRequestContentBufferSize = 67108864 });
            _httpClient.MaxResponseContentBufferSize = 67108864;
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "HttpClient");
            _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(GetLangDescription(Lang)));
        }

        private async Task AuthorizeAppAsync()
        {
            var tokenRequest = new AuthTokenRequest();
            HttpResponseMessage response = null;

            if (_grant_type == "client_credentials")
            {
                tokenRequest.Grant_Type = "client_credentials";
                tokenRequest.Client_Id = _clientId;
                tokenRequest.Client_Secret = _clientSecret;
                response = await HttpClient.PostAsync(string.Format("{0}/api/token", Endpoint),
                    new StringContent(JsonConvert.SerializeObject(tokenRequest), Encoding.UTF8, "application/json"));
            }
            else if (_grant_type == "user_key")
            {
                tokenRequest.Grant_Type = "user_key";
                tokenRequest.User_Key = _userkey;
                response = await HttpClient.PostAsync(string.Format("{0}/api/token", Endpoint),
                    new StringContent(JsonConvert.SerializeObject(tokenRequest), Encoding.UTF8, "application/json"));
            }
            else
                throw new ReachClientException { ErrorCode = ReachExceptionCodes.ClientUnknown };

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                AuthToken = JsonConvert.DeserializeObject<AuthToken>(content);
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthToken.Token_Type, AuthToken.Access_Token);
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

        #region Error Handling

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

        internal async Task<T> CatchException<T>(Exception ex, Task<T> task)
        {
            if (ex is ReachClientException)
            {
                if (((ReachClientException)ex).ErrorCode == ReachExceptionCodes.AuthTokenExpired)
                    return await task;
                throw ex;
            }
            else if (ex is HttpRequestException)
            {
                throw new ReachClientException { ErrorCode = ReachExceptionCodes.ConnectionError };
            }
            else
            {
                throw new ReachClientException { ErrorCode = ReachExceptionCodes.ClientUnknown };
            }
        }

        #endregion Error Handling
    }
}
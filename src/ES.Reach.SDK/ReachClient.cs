using Epicalsoft.Reach.Api.Client.Net.Utils;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Epicalsoft.Reach.Api.Client.Net
{
    public static class ReachClient
    {
        public const string Endpoint = "https://reachsosapis.azurewebsites.net";
        public static AuthToken AuthToken { get; private set; }
        private static HttpClient _httpClient;
        internal static HttpClient HttpClient { get { if (null == _httpClient) CreateHttpClient(); return _httpClient; } }
        private static string _clientId, _clientSecret, _userkey, _grant_type;

        public static void Init(string clientId, string clientSecret)
        {
            if (string.IsNullOrWhiteSpace(clientId))
                throw new ArgumentNullException("clientId");

            if (string.IsNullOrWhiteSpace(clientSecret))
                throw new ArgumentNullException("clientSecret");

            _grant_type = "client_credentials";
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public static void Init(string userkey)
        {
            if (string.IsNullOrWhiteSpace(userkey))
                throw new ArgumentNullException("userkey");

            _grant_type = "user_key";
            _userkey = userkey;
        }

        public static void ClearLocalCache()
        {
            LocalCachingProvider.ClearLocalCache();
        }

        internal static void CreateHttpClient()
        {
            if (string.IsNullOrWhiteSpace(_grant_type))
                throw new ReachClientException(ReachExceptionCodes.ClientError, "ReachClient.Init() has been not called.");

            _httpClient = new HttpClient(new HttpClientHandler { MaxRequestContentBufferSize = 67108864 });
            _httpClient.MaxResponseContentBufferSize = 67108864;
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "HttpClient");
            _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(CultureInfo.CurrentCulture.Name));
        }

        private async static Task AuthorizeAppAsync()
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
                throw new ReachClientException(ReachExceptionCodes.ClientError, "ReachClient.Init() has been not called.");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                AuthToken = JsonConvert.DeserializeObject<AuthToken>(content);
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthToken.Token_Type, AuthToken.Access_Token);
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new ReachClientException(ReachExceptionCodes.Forbidden);
            else
                throw new ReachClientException(ReachExceptionCodes.ServerUnknown);
        }

        internal async static Task<bool> CheckAuthorization(bool force)
        {
            if (null == AuthToken || AuthToken.IsExpiring() || force)
                await AuthorizeAppAsync();
            return true;
        }

        #region Error Handling

        internal async static Task<ReachClientException> ProcessUnsuccessResponseMessage(HttpResponseMessage response)
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
                                return new ReachClientException(ReachExceptionCodes.AuthTokenExpired);
                        }
                    }

                    return new ReachClientException(ReachExceptionCodes.Unauthorized);
                }
                else
                    return new ReachClientException(ReachExceptionCodes.ServerUnknown);
            }
            catch (Exception)
            {
                throw new ReachClientException(ReachExceptionCodes.ClientUnknown);
            }
        }

        internal async static Task<T> CatchException<T>(Exception ex, Func<Task<T>> func)
        {
            if (ex is ReachClientException)
            {
                if (((ReachClientException)ex).ErrorCode == ReachExceptionCodes.ClientUnknown)
                    throw new ReachClientException(ReachExceptionCodes.ClientUnknown, ex);
                else if (((ReachClientException)ex).ErrorCode == ReachExceptionCodes.AuthTokenExpired)
                    return await func();
                throw ex;
            }
            else if (ex is HttpRequestException)
                throw new ReachClientException(ReachExceptionCodes.ConnectionError);
            else
                throw new ReachClientException(ReachExceptionCodes.ClientUnknown, ex);
        }

        #endregion Error Handling
    }
}
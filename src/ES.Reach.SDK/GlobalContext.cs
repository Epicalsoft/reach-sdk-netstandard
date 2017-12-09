using ES.Reach.SDK.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ES.Reach.SDK
{
    public class GlobalContext
    {
        private readonly ReachClient _reachClient;

        public GlobalContext(ReachClient reachClient)
        {
            _reachClient = reachClient;
        }

        public async Task<List<IncidentSeed>> GetNearbyIncidents(double lat, double lng, byte groupId)
        {
            try
            {
                await _reachClient.CheckAuthorization();

                var httpClient = new HttpClient(new HttpClientHandler { MaxRequestContentBufferSize = 67108864 });
                httpClient.MaxResponseContentBufferSize = 67108864;
                httpClient.Timeout = TimeSpan.FromSeconds(10);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _reachClient.AuthToken.Access_Token);

                var body = JsonConvert.SerializeObject(new { Lat = lat, Lng = lng, GroupId = groupId });
                var stringContent = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://reachsosapis.azurewebsites.net/v2.0/incidents/global/nearby", stringContent);

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<List<IncidentSeed>>(await response.Content.ReadAsStringAsync());
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        throw new ReachClientException { ErrorCode = ReachExceptionCodes.Unauthorized };
                    }
                    else
                    {
                        var reachApiException = JsonConvert.DeserializeObject<ReachApiException>(content);
                        throw ReachClientException.Create(reachApiException);
                    }
                }
                else
                    throw new ReachClientException { ErrorCode = ReachExceptionCodes.Unknown };
            }
            catch (ReachClientException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ReachClientException { ErrorCode = ReachExceptionCodes.Unknown };
            }
        }

        public async Task<List<IncidentSeed>> GetIncidentDetail(int id)
        {
            try
            {
                await _reachClient.CheckAuthorization();

                var httpClient = new HttpClient(new HttpClientHandler { MaxRequestContentBufferSize = 67108864 });
                httpClient.MaxResponseContentBufferSize = 67108864;
                httpClient.Timeout = TimeSpan.FromSeconds(10);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _reachClient.AuthToken.Access_Token);
                return null;
                //var response = await httpClient.GetAsync("https://reachsosapis.azurewebsites.net/v2.0/incidents/global/nearby", stringContent);

                //if (response.IsSuccessStatusCode)
                //    return JsonConvert.DeserializeObject<List<IncidentSeed>>(await response.Content.ReadAsStringAsync());
                //else if (response.StatusCode == HttpStatusCode.Unauthorized)
                //{
                //    var content = await response.Content.ReadAsStringAsync();
                //    if (string.IsNullOrWhiteSpace(content))
                //    {
                //        throw new ReachClientException { ErrorCode = ReachExceptionCodes.Unauthorized };
                //    }
                //    else
                //    {
                //        var reachApiException = JsonConvert.DeserializeObject<ReachApiException>(content);
                //        throw ReachClientException.Create(reachApiException);
                //    }
                //}
                //else
                //    throw new ReachClientException { ErrorCode = ReachExceptionCodes.Unknown };
            }
            catch (ReachClientException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ReachClientException { ErrorCode = ReachExceptionCodes.Unknown };
            }
        }
    }
}
using ES.Reach.SDK.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
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

        #region Incidents

        public async Task<List<IncidentSeed>> GetNearbyIncidents(double lat, double lng, byte groupId)
        {
            try
            {
                await _reachClient.CheckAuthorization();
                var httpClient = _reachClient.CreateHttpClient();
                var body = JsonConvert.SerializeObject(new { Lat = lat, Lng = lng, GroupId = groupId });
                var stringContent = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://reachsosapis.azurewebsites.net/v2.0/incidents/global/nearby", stringContent);

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<List<IncidentSeed>>(await response.Content.ReadAsStringAsync());
                else
                    throw await _reachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (ReachClientException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ReachClientException { ErrorCode = ReachExceptionCodes.ClientUnknown };
            }
        }

        public async Task<Incident> GetIncidentDetail(int id)
        {
            try
            {
                await _reachClient.CheckAuthorization();
                var httpClient = _reachClient.CreateHttpClient();

                var response = await httpClient.GetAsync(string.Format("https://reachsosapis.azurewebsites.net/v2.0/incidents/global/find?id={0}", id));
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<Incident>(await response.Content.ReadAsStringAsync());
                else
                    throw await _reachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (ReachClientException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ReachClientException { ErrorCode = ReachExceptionCodes.ClientUnknown };
            }
        }

        #endregion Incidents

        #region Lists

        public async Task<List<IncidentType>> GetIncidentTypes()
        {
            try
            {
                await _reachClient.CheckAuthorization();
                var httpClient = _reachClient.CreateHttpClient();
                var response = await httpClient.GetAsync("https://reachsosapis.azurewebsites.net/v2.0/lists/incidenttypes");

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<List<IncidentType>>(await response.Content.ReadAsStringAsync());
                else
                    throw await _reachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (ReachClientException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ReachClientException { ErrorCode = ReachExceptionCodes.ClientUnknown };
            }
        }

        public async Task<List<RoadType>> GetRoadTypes()
        {
            try
            {
                await _reachClient.CheckAuthorization();
                var httpClient = _reachClient.CreateHttpClient();
                var response = await httpClient.GetAsync("https://reachsosapis.azurewebsites.net/v2.0/lists/roadtypes");

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<List<RoadType>>(await response.Content.ReadAsStringAsync());
                else
                    throw await _reachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (ReachClientException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ReachClientException { ErrorCode = ReachExceptionCodes.ClientUnknown };
            }
        }

        public async Task<List<Country>> GetCountries()
        {
            try
            {
                await _reachClient.CheckAuthorization();
                var httpClient = _reachClient.CreateHttpClient();
                var response = await httpClient.GetAsync("https://reachsosapis.azurewebsites.net/v2.0/lists/countries");

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<List<Country>>(await response.Content.ReadAsStringAsync());
                else
                    throw await _reachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (ReachClientException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ReachClientException { ErrorCode = ReachExceptionCodes.ClientUnknown };
            }
        }

        #endregion Lists

        #region Faces

        public async Task<SuspectVerifyResult> VerifySuspect(Face face)
        {
            try
            {
                await _reachClient.CheckAuthorization();
                var httpClient = _reachClient.CreateHttpClient(60);
                var stringContent = new StringContent(JsonConvert.SerializeObject(face), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://reachsosapis.azurewebsites.net/v2.0/faces/verify", stringContent);

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<SuspectVerifyResult>(await response.Content.ReadAsStringAsync());
                else
                    throw await _reachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (ReachClientException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ReachClientException { ErrorCode = ReachExceptionCodes.ClientUnknown };
            }
        }

        #endregion Faces
    }
}
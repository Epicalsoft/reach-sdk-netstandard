using Epicalsoft.Reach.Api.Client.Net.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Epicalsoft.Reach.Api.Client.Net.Contexts
{
    public class GlobalContext
    {
        private readonly ReachClient _reachClient;

        public GlobalContext(ReachClient reachClient)
        {
            _reachClient = reachClient;
        }

        #region Incidents

        public async Task<List<IncidentSeed>> GetNearbyIncidents(double lat, double lng, ClassificationGroup group)
        {
            return await GetNearbyIncidents(false, lat, lng, group);
        }

        private async Task<List<IncidentSeed>> GetNearbyIncidents(bool force, double lat, double lng, ClassificationGroup group)
        {
            try
            {
                await _reachClient.CheckAuthorization(force);

                var body = JsonConvert.SerializeObject(new { Lat = lat, Lng = lng, Group = group });
                var stringContent = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await _reachClient.HttpClient.PostAsync(string.Format("{0}/v3.0/incidents/global/nearby", _reachClient.Endpoint), stringContent);

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<List<IncidentSeed>>(await response.Content.ReadAsStringAsync());
                else
                    throw await _reachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await _reachClient.CatchException(ex, GetNearbyIncidents(true, lat, lng, group));
            }
        }

        public async Task<Incident> GetIncidentDetail(int id)
        {
            return await GetIncidentDetail(false, id);
        }

        private async Task<Incident> GetIncidentDetail(bool force, int id)
        {
            try
            {
                await _reachClient.CheckAuthorization(force);

                var response = await _reachClient.HttpClient.GetAsync(string.Format("{0}/v3.0/incidents/global/find?id={1}", _reachClient.Endpoint, id));
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<Incident>(await response.Content.ReadAsStringAsync());
                else if (response.StatusCode == HttpStatusCode.NotFound)
                    return null;
                else
                    throw await _reachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await _reachClient.CatchException(ex, GetIncidentDetail(true, id));
            }
        }

        #endregion Incidents

        #region Lists

        public async Task<List<Classification>> GetClassifications()
        {
            return await GetClassifications(false);
        }

        private async Task<List<Classification>> GetClassifications(bool force)
        {
            try
            {
                await _reachClient.CheckAuthorization(force);

                var response = await _reachClient.HttpClient.GetAsync(string.Format("{0}/v3.0/lists/classifications", _reachClient.Endpoint));
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<List<Classification>>(await response.Content.ReadAsStringAsync());
                else
                    throw await _reachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await _reachClient.CatchException(ex, GetClassifications(true));
            }
        }

        public async Task<List<RoadType>> GetRoadTypes()
        {
            return await GetRoadTypes(false);
        }

        private async Task<List<RoadType>> GetRoadTypes(bool force)
        {
            try
            {
                await _reachClient.CheckAuthorization(force);

                var response = await _reachClient.HttpClient.GetAsync(string.Format("{0}/v3.0/lists/roadtypes", _reachClient.Endpoint));

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<List<RoadType>>(await response.Content.ReadAsStringAsync());
                else
                    throw await _reachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await _reachClient.CatchException(ex, GetRoadTypes(true));
            }
        }

        public async Task<List<Country>> GetCountries()
        {
            return await GetCountries(false);
        }

        private async Task<List<Country>> GetCountries(bool force)
        {
            try
            {
                await _reachClient.CheckAuthorization(force);
                var response = await _reachClient.HttpClient.GetAsync(string.Format("{0}/v3.0/lists/countries", _reachClient.Endpoint));

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<List<Country>>(await response.Content.ReadAsStringAsync());
                else
                    throw await _reachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await _reachClient.CatchException(ex, GetCountries(true));
            }
        }

        #endregion Lists

        #region Faces

        public async Task<FacesVerificationResult> VerifyFaces(VerifyFacesRequest verifyFacesRequest)
        {
            return await VerifyFaces(false, verifyFacesRequest);
        }

        private async Task<FacesVerificationResult> VerifyFaces(bool force, VerifyFacesRequest verifyFacesRequest)
        {
            try
            {
                await _reachClient.CheckAuthorization(force);
                var stringContent = new StringContent(JsonConvert.SerializeObject(verifyFacesRequest), Encoding.UTF8, "application/json");
                var response = await _reachClient.HttpClient.PostAsync(string.Format("{0}/v3.0/faces/verify", _reachClient.Endpoint), stringContent);

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<FacesVerificationResult>(await response.Content.ReadAsStringAsync());
                else
                    throw await _reachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await _reachClient.CatchException(ex, VerifyFaces(true, verifyFacesRequest));
            }
        }

        #endregion Faces

        #region SOS

        public async Task<bool> RegisterSOSAlert(SOSAlert alert)
        {
            return await RegisterSOSAlert(false, alert);
        }

        private async Task<bool> RegisterSOSAlert(bool force, SOSAlert alert)
        {
            try
            {
                await _reachClient.CheckAuthorization(force);
                var stringContent = new StringContent(JsonConvert.SerializeObject(alert), Encoding.UTF8, "application/json");
                var response = await _reachClient.HttpClient.PostAsync(string.Format("{0}/v3.0/sos/global/alerts", _reachClient.Endpoint), stringContent);

                if (response.IsSuccessStatusCode)
                    return true;
                else
                    throw await _reachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await _reachClient.CatchException(ex, RegisterSOSAlert(true, alert));
            }
        }

        #endregion SOS
    }
}
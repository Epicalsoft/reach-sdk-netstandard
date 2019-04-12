using Epicalsoft.Reach.Api.Client.Net.Models;
using Epicalsoft.Reach.Api.Client.Net.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Epicalsoft.Reach.Api.Client.Net.Managers
{
    public class GlobalScopeManager
    {
        public static GlobalScopeManager Instance = new GlobalScopeManager();

        private GlobalScopeManager()
        {
        }

        #region Incidents

        public async Task<List<IncidentSeed>> GetNearbyIncidents(double lat, double lng, ClassificationGroup group)
        {
            return await GetNearbyIncidentsAsync(lat, lng, group);
        }

        private async Task<List<IncidentSeed>> GetNearbyIncidentsAsync(double lat, double lng, ClassificationGroup group, bool forceAuth = false)
        {
            try
            {
                await ReachClient.CheckAuthorization(forceAuth);

                var body = JsonConvert.SerializeObject(new { Lat = lat, Lng = lng, Group = group });
                var stringContent = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await ReachClient.HttpClient.PostAsync(string.Format("{0}/v4.0/incidents/global/nearby", ReachClient.Endpoint), stringContent);

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<List<IncidentSeed>>(await response.Content.ReadAsStringAsync());
                else
                    throw await ReachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await ReachClient.CatchException(ex, () => GetNearbyIncidentsAsync(lat, lng, group, true));
            }
        }

        public async Task<Incident> GetIncidentDetail(int id)
        {
            return await GetIncidentDetailAsync(id);
        }

        private async Task<Incident> GetIncidentDetailAsync(int id, bool forceAuth = false)
        {
            try
            {
                await ReachClient.CheckAuthorization(forceAuth);

                var response = await ReachClient.HttpClient.GetAsync(string.Format("{0}/v4.0/incidents/global/find?id={1}", ReachClient.Endpoint, id));
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<Incident>(await response.Content.ReadAsStringAsync());
                else if (response.StatusCode == HttpStatusCode.NotFound)
                    return null;
                else
                    throw await ReachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await ReachClient.CatchException(ex, () => GetIncidentDetailAsync(id, true));
            }
        }

        #endregion Incidents

        #region Lists

        public async Task<List<Classification>> GetClassifications()
        {
            string key = string.Format("ClassificationsState-{0}", CultureInfo.CurrentCulture.TwoLetterISOLanguageName);

            var cachingObject = await LocalCachingProvider.Instance.LoadState<CachingObject<List<Classification>>>(key);
            if (null == cachingObject || cachingObject.IsExpired)
            {
                try
                {
                    var result = await GetClassificationsAsync(false);
                    cachingObject = new CachingObject<List<Classification>>(result);
                    LocalCachingProvider.Instance.SaveState(key, cachingObject);
                }
                catch (ReachClientException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    if (null != cachingObject)
                        return cachingObject.State;

                    throw new ReachClientException(ReachExceptionCodes.ClientUnknown, ex);
                }
            }

            return cachingObject.State;
        }

        private async Task<List<Classification>> GetClassificationsAsync(bool forceAuth = false)
        {
            try
            {
                await ReachClient.CheckAuthorization(forceAuth);

                var response = await ReachClient.HttpClient.GetAsync(string.Format("{0}/v4.0/lists/classifications", ReachClient.Endpoint));
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<List<Classification>>(await response.Content.ReadAsStringAsync());
                else
                    throw await ReachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await ReachClient.CatchException(ex, () => GetClassificationsAsync(true));
            }
        }

        public async Task<List<RoadType>> GetRoadTypes()
        {
            string key = string.Format("RoadTypesState-{0}", CultureInfo.CurrentCulture.TwoLetterISOLanguageName);

            var cachingObject = await LocalCachingProvider.Instance.LoadState<CachingObject<List<RoadType>>>(key);
            if (null == cachingObject || cachingObject.IsExpired)
            {
                try
                {
                    var result = await GetRoadTypesAsync(false);
                    cachingObject = new CachingObject<List<RoadType>>(result);
                    LocalCachingProvider.Instance.SaveState(key, cachingObject);
                }
                catch (ReachClientException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    if (null != cachingObject)
                        return cachingObject.State;

                    throw new ReachClientException(ReachExceptionCodes.ClientUnknown, ex);
                }
            }

            return cachingObject.State;
        }

        private async Task<List<RoadType>> GetRoadTypesAsync(bool forceAuth = false)
        {
            try
            {
                await ReachClient.CheckAuthorization(forceAuth);

                var response = await ReachClient.HttpClient.GetAsync(string.Format("{0}/v4.0/lists/roadtypes", ReachClient.Endpoint));

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<List<RoadType>>(await response.Content.ReadAsStringAsync());
                else
                    throw await ReachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await ReachClient.CatchException(ex, () => GetRoadTypesAsync(true));
            }
        }

        public async Task<List<Country>> GetCountries()
        {
            string key = string.Format("Countries-{0}", CultureInfo.CurrentCulture.TwoLetterISOLanguageName);

            var cachingObject = await LocalCachingProvider.Instance.LoadState<CachingObject<List<Country>>>(key);
            if (null == cachingObject || cachingObject.IsExpired)
            {
                try
                {
                    var result = await GetCountriesAsync(false);
                    cachingObject = new CachingObject<List<Country>>(result);
                    LocalCachingProvider.Instance.SaveState(key, cachingObject);
                }
                catch (ReachClientException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    if (null != cachingObject)
                        return cachingObject.State;

                    throw new ReachClientException(ReachExceptionCodes.ClientUnknown, ex);
                }
            }

            return cachingObject.State;
        }

        private async Task<List<Country>> GetCountriesAsync(bool forceAuth = false)
        {
            try
            {
                await ReachClient.CheckAuthorization(forceAuth);
                var response = await ReachClient.HttpClient.GetAsync(string.Format("{0}/v4.0/lists/countries", ReachClient.Endpoint));

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<List<Country>>(await response.Content.ReadAsStringAsync());
                else
                    throw await ReachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await ReachClient.CatchException(ex, () => GetCountriesAsync(true));
            }
        }

        #endregion Lists

        #region Faces

        public async Task<FacesVerificationResult> VerifyFaces(VerifyFacesRequest verifyFacesRequest)
        {
            return await VerifyFaces(verifyFacesRequest);
        }

        private async Task<FacesVerificationResult> VerifyFacesAsync(VerifyFacesRequest verifyFacesRequest, bool forceAuth = false)
        {
            try
            {
                await ReachClient.CheckAuthorization(forceAuth);
                var stringContent = new StringContent(JsonConvert.SerializeObject(verifyFacesRequest), Encoding.UTF8, "application/json");
                var response = await ReachClient.HttpClient.PostAsync(string.Format("{0}/v4.0/faces/verify", ReachClient.Endpoint), stringContent);

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<FacesVerificationResult>(await response.Content.ReadAsStringAsync());
                else
                    throw await ReachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await ReachClient.CatchException(ex, () => VerifyFacesAsync(verifyFacesRequest, true));
            }
        }

        #endregion Faces

        #region SOS

        public async Task<bool> SendSOSAlert(SOSAlert alert)
        {
            return await SendSOSAlertAsync(alert);
        }

        private async Task<bool> SendSOSAlertAsync(SOSAlert alert, bool forceAuth = false)
        {
            try
            {
                await ReachClient.CheckAuthorization(forceAuth);
                var stringContent = new StringContent(JsonConvert.SerializeObject(alert), Encoding.UTF8, "application/json");
                var response = await ReachClient.HttpClient.PostAsync(string.Format("{0}/v4.0/sos/global/alerts", ReachClient.Endpoint), stringContent);

                if (response.IsSuccessStatusCode)
                    return true;
                else
                    throw await ReachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await ReachClient.CatchException(ex, () => SendSOSAlertAsync(alert, true));
            }
        }

        #endregion SOS
    }
}
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

        public async Task<List<IncidentSeed>> GetNearbyIncidentsAsync(double lat, double lng, ClassificationGroup group)
        {
            return await GetNearbyIncidents(lat, lng, group);
        }

        private async Task<List<IncidentSeed>> GetNearbyIncidents(double lat, double lng, ClassificationGroup group, bool forceAuth = false)
        {
            try
            {
                await ReachClient.CheckAuthorization(forceAuth);

                var body = JsonConvert.SerializeObject(new { Lat = lat, Lng = lng, Group = group });
                var stringContent = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await ReachClient.HttpClient.PostAsync(string.Format("{0}/v5.0/incidents/global/nearby", ReachClient.Endpoint), stringContent);

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<List<IncidentSeed>>(await response.Content.ReadAsStringAsync());
                else
                    throw await ReachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await ReachClient.CatchException(ex, () => GetNearbyIncidents(lat, lng, group, true));
            }
        }

        public async Task<Incident> GetIncidentDetailAsync(int id)
        {
            return await GetIncidentDetail(id);
        }

        private async Task<Incident> GetIncidentDetail(int id, bool forceAuth = false)
        {
            try
            {
                await ReachClient.CheckAuthorization(forceAuth);

                var response = await ReachClient.HttpClient.GetAsync(string.Format("{0}/v5.0/incidents/global/find?id={1}", ReachClient.Endpoint, id));
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<Incident>(await response.Content.ReadAsStringAsync());
                else if (response.StatusCode == HttpStatusCode.NotFound)
                    return null;
                else
                    throw await ReachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await ReachClient.CatchException(ex, () => GetIncidentDetail(id, true));
            }
        }

        #endregion Incidents

        #region Lists

        public async Task<List<Classification>> GetClassificationsAsync()
        {
            string key = string.Format("ClassificationsState-{0}", CultureInfo.CurrentCulture.TwoLetterISOLanguageName);

            var cachingObject = await LocalCachingProvider.Instance.LoadState<CachingObject<List<Classification>>>(key);
            if (null == cachingObject || cachingObject.IsExpired)
            {
                try
                {
                    var result = await GetClassifications(false);
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

        private async Task<List<Classification>> GetClassifications(bool forceAuth = false)
        {
            try
            {
                await ReachClient.CheckAuthorization(forceAuth);

                var response = await ReachClient.HttpClient.GetAsync(string.Format("{0}/v5.0/lists/classifications", ReachClient.Endpoint));
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<List<Classification>>(await response.Content.ReadAsStringAsync());
                else
                    throw await ReachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await ReachClient.CatchException(ex, () => GetClassifications(true));
            }
        }

        public async Task<List<RoadType>> GetRoadTypesAsync()
        {
            string key = string.Format("RoadTypesState-{0}", CultureInfo.CurrentCulture.TwoLetterISOLanguageName);

            var cachingObject = await LocalCachingProvider.Instance.LoadState<CachingObject<List<RoadType>>>(key);
            if (null == cachingObject || cachingObject.IsExpired)
            {
                try
                {
                    var result = await GetRoadTypes(false);
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

        private async Task<List<RoadType>> GetRoadTypes(bool forceAuth = false)
        {
            try
            {
                await ReachClient.CheckAuthorization(forceAuth);

                var response = await ReachClient.HttpClient.GetAsync(string.Format("{0}/v5.0/lists/roadtypes", ReachClient.Endpoint));

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<List<RoadType>>(await response.Content.ReadAsStringAsync());
                else
                    throw await ReachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await ReachClient.CatchException(ex, () => GetRoadTypes(true));
            }
        }

        public async Task<List<Country>> GetCountriesAsync()
        {
            string key = string.Format("Countries-{0}", CultureInfo.CurrentCulture.TwoLetterISOLanguageName);

            var cachingObject = await LocalCachingProvider.Instance.LoadState<CachingObject<List<Country>>>(key);
            if (null == cachingObject || cachingObject.IsExpired)
            {
                try
                {
                    var result = await GetCountries(false);
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

        private async Task<List<Country>> GetCountries(bool forceAuth = false)
        {
            try
            {
                await ReachClient.CheckAuthorization(forceAuth);
                var response = await ReachClient.HttpClient.GetAsync(string.Format("{0}/v5.0/lists/countries", ReachClient.Endpoint));

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<List<Country>>(await response.Content.ReadAsStringAsync());
                else
                    throw await ReachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await ReachClient.CatchException(ex, () => GetCountries(true));
            }
        }

        #endregion Lists

        #region Faces

        public async Task<VerifyFacesResult> VerifyFacesAsync(MediaFileData mediaFileData)
        {
            if (mediaFileData.Kind == MediaFileKind.Audio)
                throw new ReachClientException(ReachExceptionCodes.ClientError, "This media file kind is not supported.");

            return await VerifyFaces(mediaFileData);
        }

        private async Task<VerifyFacesResult> VerifyFaces(MediaFileData mediaFileData, bool forceAuth = false)
        {
            try
            {
                await ReachClient.CheckAuthorization(forceAuth);
                var stringContent = new StringContent(JsonConvert.SerializeObject(mediaFileData), Encoding.UTF8, "application/json");
                var response = await ReachClient.HttpClient.PostAsync(string.Format("{0}/v5.0/faces/verify", ReachClient.Endpoint), stringContent);

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<VerifyFacesResult>(await response.Content.ReadAsStringAsync());
                else
                    throw await ReachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await ReachClient.CatchException(ex, () => VerifyFaces(mediaFileData, true));
            }
        }

        #endregion Faces

        #region SOS

        public async Task<bool> SendSOSAlertAsync(SOSAlert alert)
        {
            return await SendSOSAlert(alert);
        }

        private async Task<bool> SendSOSAlert(SOSAlert alert, bool forceAuth = false)
        {
            try
            {
                await ReachClient.CheckAuthorization(forceAuth);
                var stringContent = new StringContent(JsonConvert.SerializeObject(alert), Encoding.UTF8, "application/json");
                var response = await ReachClient.HttpClient.PostAsync(string.Format("{0}/v5.0/sos/global/alerts", ReachClient.Endpoint), stringContent);

                if (response.IsSuccessStatusCode)
                    return true;
                else
                    throw await ReachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await ReachClient.CatchException(ex, () => SendSOSAlert(alert, true));
            }
        }

        #endregion SOS
    }
}
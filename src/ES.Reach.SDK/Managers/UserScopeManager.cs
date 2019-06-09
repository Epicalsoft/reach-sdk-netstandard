using Epicalsoft.Reach.Api.Client.Net.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Epicalsoft.Reach.Api.Client.Net.Managers
{
    public class UserScopeManager
    {
        public static UserScopeManager Instance = new UserScopeManager();

        private UserScopeManager()
        {
        }

        #region MediaFiles

        public async Task<MediaFileUploadResult> UploadMediaFileAsync(MediaFileData mediaFileData)
        {
            return await UploadMediaFile(mediaFileData);
        }

        private async Task<MediaFileUploadResult> UploadMediaFile(MediaFileData mediaFileData, bool forceAuth = false)
        {
            try
            {
                await ReachClient.CheckAuthorization(forceAuth);

                var body = JsonConvert.SerializeObject(mediaFileData);
                var stringContent = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await ReachClient.HttpClient.PostAsync(string.Format("{0}/v5.0/mediafiles/user/upload", ReachClient.Endpoint), stringContent);

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<MediaFileUploadResult>(await response.Content.ReadAsStringAsync());
                else
                    throw await ReachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await ReachClient.CatchException(ex, () => UploadMediaFile(mediaFileData, true));
            }
        }

        #endregion MediaFiles

        #region Incidents

        public async Task<int> PostIncidentAsync(IncidentPost incidentPost)
        {
            return await PostIncident(incidentPost);
        }

        private async Task<int> PostIncident(IncidentPost incidentPost, bool forceAuth = false)
        {
            try
            {
                await ReachClient.CheckAuthorization(forceAuth);

                var body = JsonConvert.SerializeObject(incidentPost);
                var stringContent = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await ReachClient.HttpClient.PostAsync(string.Format("{0}/v5.0/incidents/user/publish", ReachClient.Endpoint), stringContent);

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
                else
                    throw await ReachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await ReachClient.CatchException(ex, () => PostIncident(incidentPost, true));
            }
        }

        #endregion Incidents
    }
}
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

        #region Incidents

        public async Task<int> PostIncident(IncidentPost incidentPost)
        {
            return await PostIncidentAsync(incidentPost);
        }

        private async Task<int> PostIncidentAsync(IncidentPost incidentPost, bool forceAuth = false)
        {
            try
            {
                await ReachClient.CheckAuthorization(forceAuth);

                var body = JsonConvert.SerializeObject(incidentPost);
                var stringContent = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await ReachClient.HttpClient.PostAsync(string.Format("{0}/v4.0/incidents/user/publish", ReachClient.Endpoint), stringContent);

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
                else
                    throw await ReachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await ReachClient.CatchException(ex, () => PostIncidentAsync(incidentPost, true));
            }
        }

        #endregion Incidents
    }
}
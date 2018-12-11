using Epicalsoft.Reach.Api.Client.Net.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Epicalsoft.Reach.Api.Client.Net.Contexts
{
    public class UserContext
    {
        private readonly ReachClient _reachClient;

        public UserContext(ReachClient reachClient)
        {
            _reachClient = reachClient;
        }

        #region Incidents

        public async Task<int> PostIncident(IncidentPost incidentPost)
        {
            return await PostIncident(false, incidentPost);
        }

        private async Task<int> PostIncident(bool force, IncidentPost incidentPost)
        {
            try
            {
                await _reachClient.CheckAuthorization(force);

                var body = JsonConvert.SerializeObject(incidentPost);
                var stringContent = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await _reachClient.HttpClient.PostAsync(string.Format("{0}/v3.0/incidents/user/publish", _reachClient.Endpoint), stringContent);

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
                else
                    throw await _reachClient.ProcessUnsuccessResponseMessage(response);
            }
            catch (Exception ex)
            {
                return await _reachClient.CatchException(ex, PostIncident(true, incidentPost));
            }
        }

        #endregion Incidents
    }
}
using NBAStats.Models.TeamsModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NBAStats.Services
{
    public class NbaApiService : INbaApiService
    {
        public Config Url;
        public async Task<TeamsList> GetTeamsInformation()
        {
            TeamsList retuncalender = null;
            HttpClient client = new HttpClient();
            var CalenderInfo = await client.GetAsync(Url.GetTeamsUrl());

            if (CalenderInfo.IsSuccessStatusCode)
            {
                var teams = await CalenderInfo.Content.ReadAsStringAsync();
                retuncalender = JsonConvert.DeserializeObject<TeamsList>(teams);

            }
            return retuncalender;
        }
    }
    
}

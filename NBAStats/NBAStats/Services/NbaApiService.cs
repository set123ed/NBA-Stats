using NBAStats.Models.PlayersModel;
using NBAStats.Models.TeamsModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Text.Json;
using NBAStats.Models.CoachModels;
using static NBAStats.Models.CoachModels.Coach;

namespace NBAStats.Services
{
    public class NbaApiService : INbaApiService
    {
        private Config url = new Config();

        public async Task<CoachInfo> GetCoachList()
        {
            CoachInfo retVal = null;
            HttpClient client = new HttpClient();

            var coachResponse = await client.GetAsync(url.GetCoachUrl());
            if (coachResponse.IsSuccessStatusCode)
            {
                var jsonPayload = await coachResponse.Content.ReadAsStringAsync();
                retVal = System.Text.Json.JsonSerializer.Deserialize<CoachInfo>(jsonPayload);
            }
            return retVal;

        }

        public async Task<PlayerList> GetNbaPlayers()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                PlayerList players = null;

                HttpClient client = new HttpClient();

                var playersResponse = await client.GetAsync(url.GetPlayersUrl());

                if (playersResponse.IsSuccessStatusCode)
                {
                    var jsonPlayers = await playersResponse.Content.ReadAsStringAsync();
                    players = System.Text.Json.JsonSerializer.Deserialize<PlayerList>(jsonPlayers);
                }

                return players;

            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<TeamsList> GetTeamsInformation()
        {
            TeamsList teamList = null;
            HttpClient client = new HttpClient();
            var teamListInfo = await client.GetAsync(url.GetTeamsUrl());

            if (teamListInfo.IsSuccessStatusCode)
            {
                var teams = await teamListInfo.Content.ReadAsStringAsync();
                teamList = JsonConvert.DeserializeObject<TeamsList>(teams);

            }
            return teamList;
        }

        
    }
    
}

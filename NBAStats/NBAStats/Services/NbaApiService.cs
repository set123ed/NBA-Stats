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

namespace NBAStats.Services
{
    public class NbaApiService : INbaApiService
    {
        public Config Url;

        public async Task<PlayerList> GetNbaPlayers()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                PlayerList players = null;

                HttpClient client = new HttpClient();

                var playersResponse = await client.GetAsync(Url.GetPlayersUrl()); ;

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

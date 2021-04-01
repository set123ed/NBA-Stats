using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NBAStats.Models;
using Xamarin.Essentials;

namespace NBAStats.Services
{
    public class NbaApiService : INbaApiService
    {
        public async Task<SeasonRange> GetSeasonRange()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                SeasonRange seasonRange = null;

                HttpClient client = new HttpClient();

                var seasonRangeResponse = await client.GetAsync("http://data.nba.net/data/10s/prod/v1/calendar.json");

                if (seasonRangeResponse.IsSuccessStatusCode)
                {
                    var jsonTeams = await seasonRangeResponse.Content.ReadAsStringAsync();
                    seasonRange = JsonSerializer.Deserialize<SeasonRange>(jsonTeams);
                }
                return seasonRange;

            }
            else
            {
                throw new NotImplementedException();

            }


        }

        public async Task<Teams> GetTeams()
        {

            Teams teams = null;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                HttpClient client = new HttpClient();

                string urlApi = $"http://data.nba.net/data/10s/prod/v1/{DateTime.Today.Year - 1}/teams.json";

                var teamsResponse = await client.GetAsync(urlApi);

                if (teamsResponse.IsSuccessStatusCode)
                {
                    var jsonTeams = await teamsResponse.Content.ReadAsStringAsync();
                    teams = JsonSerializer.Deserialize<Teams>(jsonTeams);
                }


            }

            return teams;

        }

        public async Task<GameOfDay> GetGamesOfDay(string date)
        {
            GameOfDay gamesOfDay = null;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {


                HttpClient client = new HttpClient();

                string apiUrl = $"http://data.nba.net/data/10s/prod/v1/{date}/scoreboard.json";

                var gamesOfDayResponse = await client.GetAsync(apiUrl);

                if (gamesOfDayResponse.IsSuccessStatusCode)
                {
                    var jsonGames = await gamesOfDayResponse.Content.ReadAsStringAsync();
                    gamesOfDay = JsonSerializer.Deserialize<GameOfDay>(jsonGames);
                }


            }

            return gamesOfDay;
            //else
            //{
            //    throw new NotImplementedException();
            //}

        }

        public async Task<TeamLeaders> GetTeamLeaders(string year, string team)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                TeamLeaders teamLeaders = null;

                HttpClient client = new HttpClient();

                string urlApi = $"http://data.nba.net/data/10s/prod/v1/{year}/teams/{team}/leaders.json";

                var teamLeadersResponse = await client.GetAsync(urlApi);

                if (teamLeadersResponse.IsSuccessStatusCode)
                {
                    var jsonTeamLeaders = await teamLeadersResponse.Content.ReadAsStringAsync();
                    teamLeaders = JsonSerializer.Deserialize<TeamLeaders>(jsonTeamLeaders);
                }
                return teamLeaders;

            }
            else
            {
                throw new NotImplementedException();

            }
        }

        public async Task<PlayerStatsLeaders> GetPlayerStatsLeaders()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                PlayerStatsLeaders playerStatsLeaders = null;

                HttpClient client = new HttpClient();

                //string urlApi = $"http://data.nba.net/data/10s/prod/v1/{year}/teams/{team}/leaders.json";

                var playerStatsLeaderResponse = await client.GetAsync("https://stats.nba.com/stats/leagueleaders?LeagueID=00&PerMode=PerGame&Scope=S&Season=2020-21&SeasonType=Regular+Season&StatCategory=PTS");

                if (playerStatsLeaderResponse.IsSuccessStatusCode)
                {
                    var jsonPlayerStatsLeaders = await playerStatsLeaderResponse.Content.ReadAsStringAsync();
                    playerStatsLeaders = JsonSerializer.Deserialize<PlayerStatsLeaders>(jsonPlayerStatsLeaders);
                }
                return playerStatsLeaders;

            }
            else
            {
                throw new NotImplementedException();

            }
        }

        public async Task<Standing> GetStanding()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Standing standing= null;

                HttpClient client = new HttpClient();

                //string urlApi = $"http://data.nba.net/data/10s/prod/v1/{year}/teams/{team}/leaders.json";

                var playerStatsLeaderResponse = await client.GetAsync("http://data.nba.net/data/10s/prod/v1/current/standings_all.json");

                if (playerStatsLeaderResponse.IsSuccessStatusCode)
                {
                    var jsonStanding = await playerStatsLeaderResponse.Content.ReadAsStringAsync();
                    standing = JsonSerializer.Deserialize<Standing>(jsonStanding);
                }
                return standing;

            }
            else
            {
                throw new NotImplementedException();

            }
        }

        public async Task<TeamStatsClass> GetTeamStats()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                TeamStatsClass teamStatsClass = null;

                HttpClient client = new HttpClient();

                //string urlApi = $"http://data.nba.net/data/10s/prod/v1/{year}/teams/{team}/leaders.json";

                var teamStatsResponse = await client.GetAsync("http://data.nba.net/data/10s/prod/v1/2020/team_stats_rankings.json");

                if (teamStatsResponse.IsSuccessStatusCode)
                {
                    var jsonTeamStats = await teamStatsResponse.Content.ReadAsStringAsync();
                    teamStatsClass = JsonSerializer.Deserialize<TeamStatsClass>(jsonTeamStats);
                }
                return teamStatsClass;

            }
            else
            {
                throw new NotImplementedException();

            }
        }

        public async Task<BoxScore> GetBoxScore(string date, string gameId)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                BoxScore boxScore = null;

                HttpClient client = new HttpClient();

                string urlApi = $"http://data.nba.net/data/10s/prod/v1/{date}/{gameId}_boxscore.json";

                var boxScoreResponse = await client.GetAsync(urlApi);

                if (boxScoreResponse.IsSuccessStatusCode)
                {
                    var jsonBoxScore = await boxScoreResponse.Content.ReadAsStringAsync();
                    boxScore = JsonSerializer.Deserialize<BoxScore>(jsonBoxScore);
                }
                return boxScore;

            }
            else
            {
                throw new NotImplementedException();

            }
        }

        public async Task<Players> GetNbaPlayers()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Players players = null;
                HttpClient client = new HttpClient();

                var playersResponse = await client.GetAsync("http://data.nba.net/data/10s/prod/v1/2020/players.json");

                if (playersResponse.IsSuccessStatusCode)
                {
                    var jsonPlayers = await playersResponse.Content.ReadAsStringAsync();
                    players = JsonSerializer.Deserialize<Players>(jsonPlayers);
                }

                return players;

            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<PlayerProfile> GetPlayerProfile(string personId)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                PlayerProfile playerProfile = null;
                HttpClient client = new HttpClient();

                var playerProfileResponse = await client.GetAsync($"http://data.nba.net/data/10s/prod/v1/2020/players/{personId}_profile.json");

                if (playerProfileResponse.IsSuccessStatusCode)
                {
                    var jsonPlayerProfile = await playerProfileResponse.Content.ReadAsStringAsync();
                    playerProfile = JsonSerializer.Deserialize<PlayerProfile>(jsonPlayerProfile);
                }

                return playerProfile;

            }
            else
            {
                throw new Exception();
            }
        }
    }
}

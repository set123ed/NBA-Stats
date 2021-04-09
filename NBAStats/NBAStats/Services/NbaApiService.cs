using System;
using System.Collections.Generic;
using System.Globalization;
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

                var seasonRangeResponse = await client.GetAsync(Config.CalendarUrl);

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

        public async Task<Teams> GetTeams(string year)
        {

            Teams teams = null;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                HttpClient client = new HttpClient();

                var teamsResponse = await client.GetAsync(Config.GetTeamsUrl(year));

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

                var gamesOfDayResponse = await client.GetAsync(Config.GetScoreboardUrl(date));

                if (gamesOfDayResponse.IsSuccessStatusCode)
                {
                    var jsonGames = await gamesOfDayResponse.Content.ReadAsStringAsync();
                    gamesOfDay = JsonSerializer.Deserialize<GameOfDay>(jsonGames);
                }
                return gamesOfDay;

            }
            else
            {
                throw new NotImplementedException();
            }

        }

        public async Task<TeamLeaders> GetTeamLeaders(string year, string teamName)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                TeamLeaders teamLeaders = null;

                HttpClient client = new HttpClient();

                var teamLeadersResponse = await client.GetAsync(Config.GetTeamLeadersUrl(year,teamName));

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

        public async Task<PlayerStatsLeaders> GetPlayerStatsLeaders(string season, string stat)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                PlayerStatsLeaders playerStatsLeaders = null;

                HttpClient client = new HttpClient();

                var playerStatsLeaderResponse = await client.GetAsync(Config.GetPlayerStatsLeadersUrl(season,stat));
                
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

                var playerStatsLeaderResponse = await client.GetAsync(Config.StandingUrl);

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

        public async Task<TeamStatsClass> GetTeamStats(string year)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                TeamStatsClass teamStatsClass = null;

                HttpClient client = new HttpClient();

                var teamStatsResponse = await client.GetAsync(Config.GetTeamStatsUrl(year));

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

                var boxScoreResponse = await client.GetAsync(Config.GetBoxScoreUrl(date, gameId));

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

        public async Task<Players> GetNbaPlayers(string year)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Players players = null;
                HttpClient client = new HttpClient();

                var playersResponse = await client.GetAsync(Config.GetPlayersUrl(year));

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

        public async Task<PlayerProfile> GetPlayerProfile(string year, string personId)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                PlayerProfile playerProfile = null;
                HttpClient client = new HttpClient();

                var playerProfileResponse = await client.GetAsync(Config.GetPlayerProfileUrl(year, personId));

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

        public async Task<TeamSchedule> GetTeamSchedule(string year, string teamName)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                TeamSchedule teamSchedule = null;
                HttpClient client = new HttpClient();

                var teamScheduleResponse = await client.GetAsync(Config.GetTeamScheduleUrl(year, teamName));

                if (teamScheduleResponse.IsSuccessStatusCode)
                {
                    var jsonTeamSchedule = await teamScheduleResponse.Content.ReadAsStringAsync();
                    teamSchedule = JsonSerializer.Deserialize<TeamSchedule>(jsonTeamSchedule);
                }

                return teamSchedule;

            }
            else
            {
                throw new Exception();
            }
        }
    }
}

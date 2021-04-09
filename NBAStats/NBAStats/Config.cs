using NBAStats.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats
{
    public static class Config
    {
        public const string DataNbaApi = "http://data.nba.net";
        public const string StatsNbaApi = "https://stats.nba.com";

        public const string CalendarUrl = DataNbaApi + "/data/10s/prod/v1/calendar.json";
        public const string StandingUrl = DataNbaApi + "/data/10s/prod/v1/current/standings_all.json";

        public static List<SeasonStage> SeasonStages = new List<SeasonStage>
        {
                new SeasonStage(1,"Pre-Season"),
                new SeasonStage(2,"Regular Season"),
                new SeasonStage(3, "All Star"),
                new SeasonStage(4, "Playoff"),
                new SeasonStage(5, "Play-In")
        };

        public static string GetScoreboardUrl(string date)
        {
            return DataNbaApi + $"/data/10s/prod/v1/{date}/scoreboard.json";
        }

        public static string GetPlayerProfileUrl(string year, string personId)
        {
            return DataNbaApi + $"/data/10s/prod/v1/{year}/players/{personId}_profile.json";
        }

        public static string GetPlayerStatsLeadersUrl(string season, string stat)
        {
            return StatsNbaApi + $"/stats/leagueleaders?LeagueID=00&PerMode=PerGame&Scope=S&Season={season}&SeasonType=Regular+Season&StatCategory={stat}";
        }

        public static string GetPlayersUrl(string year)
        {
            return DataNbaApi + $"/data/10s/prod/v1/{year}/players.json";
        } 
        
        public static string GetTeamScheduleUrl(string year, string teamName)
        {
            return DataNbaApi + $"/data/10s/prod/v1/{year}/teams/{teamName}/schedule.json";
        }

        public static string GetTeamsUrl(string year)
        {
            return DataNbaApi + $"/data/10s/prod/v1/{year}/teams.json";
        }

        public static string GetCoachUrl(string year)
        {
            return DataNbaApi + $"/data/10s/prod/v1/{year}/coaches.json";
        }

        public static string GetTodayBoxScoreUrl(string date, string gameId)
        {
            return DataNbaApi + $"/data/10s/prod/v1/{date}/{gameId}_boxscore.json";
        }

        public static string GetBoxScoreUrl(string date, string gameId)
        {
            return DataNbaApi + $"/data/10s/prod/v1/{date}/{gameId}_boxscore.json";
        }

        public static string GetTeamStatsUrl(string year)
        {
            return DataNbaApi + $"/data/10s/prod/v1/{year}/team_stats_rankings.json";
        }

        public static string GetTeamLeadersUrl(string year, string teamName)
        {
            return DataNbaApi + $"/data/10s/prod/v1/{year}/teams/{teamName}/leaders.json";
        }

        public static string GetTeamRosterUrl(string year, string teamName)
        {
            return DataNbaApi + $"/data/10s/prod/v1/{year}/teams/{teamName}/roster.json";
        }
    }
}

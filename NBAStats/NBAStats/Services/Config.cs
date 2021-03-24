﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats.Services
{
    public class Config
    {
        public const string HomePage = "Home";
        public const string LoginPage = "Login";
        public const string RegisterPage = "Register";
        public const string StatsPage = "Stats";
        public const string TeamPage = "Team";
        public const string BoxScorePage = "BoxScore";
        public const string CalendarPage = "Calendar";
        public const string CoachPage = "Coach";
        public const string FavoriteTeamPage = "FavoriteTeam";
        public const string FavoritePlayerPage = "FavoritePlayer";
        public const string PlayerProfilePage = "PlayerProfile";


        public static string date = DateTime.Today.ToString("yyyyMMdd");
        public static string year = DateTime.Today.ToString("yyyy");

        public const string DataNbaApi = "http://data.nba.net";

        public string GetScoreboardUrl()
        {
            return DataNbaApi + $"/data/10s/prod/v1/{date}/scoreboard.json";
        }

        public string GetPlayerProfileUrl(string personId)
        {
            return DataNbaApi + $"/data/10s/prod/v1/{year}/players/{personId}_profile.json";
        }

        public string GetPlayersUrl()
        {
            return DataNbaApi + $"/data/10s/prod/v1/{year}/players.json";
        }

        public string GetTeamsUrl()
        {
            return DataNbaApi + $"/data/10s/prod/v1/{year}/teams.json";
        }

        public string GetCoachUrl()
        {
            return DataNbaApi + $"/data/10s/prod/v1/{year}/coaches.json";
        }

        public string GetStandingsUrl()
        {
            return DataNbaApi + $"/data/10s/prod/v1/{date}/standings_all.json";
        }

        public string GetTodayBoxScoreUrl(string gameId)
        {
            return DataNbaApi + $"/data/10s/prod/v1/{date}/{gameId}_boxscore.json";
        }

        public string GetBoxScoreUrl(string date, string gameId)
        {
            return DataNbaApi + $"/data/10s/prod/v1/{date}/{gameId}_boxscore.json";
        }

        public string GetTeamStatsLeaderUrl()
        {
            return DataNbaApi + $"/data/10s/prod/v1/{year}/team_stats_rankings.json";
        }

        public string GetTeamRosterUrl(string teamName)
        {
            return DataNbaApi + $"/data/10s/prod/v1/{year}/teams/{teamName}/roster.json";
        }
    }
}
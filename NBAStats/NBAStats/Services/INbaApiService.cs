using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NBAStats.Models;

namespace NBAStats.Services
{
    public interface INbaApiService
    {
        Task<SeasonRange> GetSeasonRange();
        Task<GameOfDay> GetGamesOfDay(string date);
        Task<Teams> GetTeams(string year);
        Task<TeamLeaders> GetTeamLeaders(string year, string team);
        Task<PlayerStatsLeaders> GetPlayerStatsLeaders(string season, string stat);
        Task<Standing> GetStanding();
        Task<TeamStatsClass> GetTeamStats(string year);
        Task<BoxScore> GetBoxScore(string date, string gameId);
        Task<Players> GetNbaPlayers(string year);
        Task<PlayerProfile> GetPlayerProfile(string year, string personId);
        Task<TeamSchedule> GetTeamSchedule(string year, string teamName);
    }
}

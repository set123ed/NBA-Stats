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
        Task<Teams> GetTeams();
        Task<TeamLeaders> GetTeamLeaders(string year, string team);
        Task<PlayerStatsLeaders> GetPlayerStatsLeaders(string season, string stat);
        Task<Standing> GetStanding();
        Task<TeamStatsClass> GetTeamStats();
        Task<BoxScore> GetBoxScore(string date, string gameId);
        Task<Players> GetNbaPlayers();
        Task<PlayerProfile> GetPlayerProfile(string personId);
    }
}

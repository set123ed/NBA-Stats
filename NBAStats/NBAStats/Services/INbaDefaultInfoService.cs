using NBAStats.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NBAStats.Services
{
    public interface INbaDefaultInfoService
    {
        Task<string> GetSeasonYearApiData();
        Task<string> GetSeasonRangeApiStat();
        Task<List<Team>> GetTeamList();
        Task<List<Player>> GetPlayerList();

    }
}
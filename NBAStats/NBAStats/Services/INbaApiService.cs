using NBAStats.Models.CoachModels;
using NBAStats.Models.PlayersModel;
using NBAStats.Models.TeamsModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static NBAStats.Models.CoachModels.Coach;

namespace NBAStats.Services
{
    public interface INbaApiService
    {
        Task<PlayerList> GetNbaPlayers();
        Task<TeamsList> GetTeamsInformation();
        Task<CoachInfo> GetCoachList();
    }
}

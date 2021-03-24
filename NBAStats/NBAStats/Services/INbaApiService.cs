using NBAStats.Models.TeamsModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NBAStats.Services
{
    public interface INbaApiService
    {
        Task<TeamsList> GetTeamsInformation();
    }
}

using NBAStats.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NBAStats.Services
{
    public interface IDataBaseServices
    {
        Task<int> SaveTeam(FavoriteTeam favoriteTeam);
        Task<int> SavePlayer(FavoritePlayer favoritePlayer);

        Task<FavoritePlayer> GetPlayerById(string idpalyer);
        Task<FavoriteTeam> GetTeamById(string idteam);

        Task<List<FavoritePlayer>> GetFavoritePalyer();

        Task<List<FavoriteTeam>> GetFavoriteTeams();

        Task<int> DeleteFavoriteTeams(FavoriteTeam favoriteTeam);

        Task<int> DeleteFavoritePlayer(FavoritePlayer favoritePlayer);


    }
}

using NBAStats.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NBAStats.Services
{
    public interface IDatabaseService
    {
        Task<int> SaveTeam(FavoritesTeam favoriteTeam);
        Task<int> SavePlayer(FavoritesPlayer favoritePlayer);

        Task<FavoritesPlayer> GetPlayerById(string idpalyer);
        Task<FavoritesTeam> GetTeamById(string idteam);

        Task<bool> FavoritePlayerExists(FavoritesPlayer favoritesPlayer);
        Task<bool> FavoriteTeamExists(FavoritesTeam favoritesTeam);

        Task<List<FavoritesPlayer>> GetFavoritePlayers();

        Task<List<FavoritesTeam>> GetFavoriteTeams();

        Task<int> DeleteFavoriteTeams(FavoritesTeam favoriteTeam);

        Task<int> DeleteFavoritePlayer(FavoritesPlayer favoritePlayer);
    }
}

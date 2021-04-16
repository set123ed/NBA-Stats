using NBAStats.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace NBAStats.Services
{
    public class DatabaseServices : IDatabaseService
    {
        private bool _initialized = false;
        private static SQLiteAsyncConnection db;

        public List<FavoritesPlayer> FavoritesPlayers { get; set; }
        public List<FavoritesTeam> FavoritesTeams { get; set; }
        public DatabaseServices()
        {
            Initialize();
        }
        private async void Initialize()
        {
            try
            {
                if (!_initialized)
                {
                    _initialized = true;

                    if (db != null)
                    {
                        return;
                    }

                    var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Favorites.db");

                    db = new SQLiteAsyncConnection(databasePath);

                    await db.CreateTableAsync<FavoritesPlayer>();
                    await db.CreateTableAsync<FavoritesTeam>();

                    FavoritesTeams = await db.Table<FavoritesTeam>().ToListAsync();
                    FavoritesPlayers = await db.Table<FavoritesPlayer>().ToListAsync();
                }
            }
            catch (Exception e)
            {

            }
        }

        public async Task<int> SaveTeam(FavoritesTeam favoriteTeam)
        {
            try
            {
                int result = await db.InsertAsync(favoriteTeam);
                FavoritesTeams = await db.Table<FavoritesTeam>().ToListAsync();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> SavePlayer(FavoritesPlayer favoritePlayer)
        {
            try
            {
                int result = await db.InsertAsync(favoritePlayer);
                FavoritesPlayers = await db.Table<FavoritesPlayer>().ToListAsync();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<FavoritesPlayer> GetPlayerById(string playerId)
        {
            return await db.Table<FavoritesPlayer>().FirstOrDefaultAsync(player => player.PlayerId == playerId);
        }

        public async Task<FavoritesTeam> GetTeamById(string teamId)
        {
            return await db.Table<FavoritesTeam>().FirstOrDefaultAsync(team => team.TeamId == teamId);
        }

        public async Task<List<FavoritesPlayer>> GetFavoritePlayers()
        {
            return await db.Table<FavoritesPlayer>().ToListAsync();
        }

        public async Task<List<FavoritesTeam>> GetFavoriteTeams()
        {
            return await db.Table<FavoritesTeam>().ToListAsync();
        }

        public async Task<int> DeleteFavoriteTeams(FavoritesTeam favoriteTeam)
        {
            int result = await db.DeleteAsync(favoriteTeam);
            FavoritesTeams = await db.Table<FavoritesTeam>().ToListAsync();

            return result;
        }

        public async Task<int> DeleteFavoritePlayer(FavoritesPlayer favoritePlayer)
        {
            int result = await db.DeleteAsync(favoritePlayer);
            FavoritesPlayers = await db.Table<FavoritesPlayer>().ToListAsync();

            return result;
        }

        public async Task<bool> FavoritePlayerExists(FavoritesPlayer player)
        {
            List<FavoritesPlayer> favoritesPlayers = await db.Table<FavoritesPlayer>().ToListAsync();

            if (favoritesPlayers.Contains(player))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> FavoriteTeamExists(FavoritesTeam team)
        {
            List<FavoritesTeam> favoritesTeams = await db.Table<FavoritesTeam>().ToListAsync();
            if (favoritesTeams.Contains(team))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

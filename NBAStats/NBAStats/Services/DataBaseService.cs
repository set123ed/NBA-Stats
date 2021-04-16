﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NBAStats.Models;
using System.Data.SQLite;
using SQLite;
using System.IO;
using Xamarin.Essentials;

namespace NBAStats.Services
{
    public class DataBaseService : IDataBaseServices
    {
        public SQLiteAsyncConnection DataBase;

        public DataBaseService(string path)
        {
            DataBase = new SQLiteAsyncConnection(path);
            DataBase.CreateTableAsync<FavoriteTeam>().Wait();
            DataBase.CreateTableAsync<FavoritePlayer>().Wait();

        }

        public Task<int> DeleteFavoritePlayer(FavoritePlayer favoritePlayer)
        {
            return DataBase.DeleteAsync(favoritePlayer);
        }

        public Task<int> DeleteFavoriteTeams(FavoriteTeam favoriteTeam)
        {
            return DataBase.DeleteAsync(favoriteTeam);
        }

        public Task<List<FavoritePlayer>> GetFavoritePalyer()
        {
            return DataBase.Table<FavoritePlayer>().ToListAsync();
        }

        public Task<List<FavoriteTeam>> GetFavoriteTeams()
        {
            return DataBase.Table<FavoriteTeam>().ToListAsync();
        }

        public Task<FavoritePlayer> GetPlayerById(string idpalyer)
        {
            return DataBase.Table<FavoritePlayer>().Where(Player => Player.IdFavoritePlayer == idpalyer).FirstOrDefaultAsync();
        }

        public Task<FavoriteTeam> GetTeamById(string idteam)
        {
            return DataBase.Table<FavoriteTeam>().Where(Team => Team.IdFavoriteTeam == idteam).FirstOrDefaultAsync();
        }

        public Task<int> SavePlayer(FavoritePlayer favoritePlayer)
        {
            if(favoritePlayer != null){

                return DataBase.InsertAsync(favoritePlayer);
            }
            else
            {
                return null;
            }
        }

        public Task<int> SaveTeam(FavoriteTeam favoriteTeam)
        {
            if(favoriteTeam != null)
            {
                return DataBase.InsertAsync(favoriteTeam);
            }
            else
            {
                return null;
            }
        }
    }
}

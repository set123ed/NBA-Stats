using NBAStats.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Prism.Navigation;

namespace NBAStats.Services
{
    public class NbaDefaultInfoService : INbaDefaultInfoService
    {
        private List<Player> _playerList = new List<Player>();
        private List<Team> _teamList = new List<Team>();

        private string _seasonYearApiData = null;
        private string _seasonApiStats = null;

        private bool _isGetTeamsRunning = false;
        private bool _isGetPlayersRunning = false;
        private bool _isGetSeasonParametersRunning = false;

        private NbaApiService _nbaApiService = new NbaApiService();

        public NbaDefaultInfoService()
        {
            GetData();
        }

        public async Task<List<Player>> GetPlayerList()
        {
            if (_isGetPlayersRunning)
            {
                await Task.Delay(3000);
            }

            if (_playerList.Count == 0)
            {
                await GetNbaPlayers();
            }

            return _playerList;
        }
        public async Task<List<Team>> GetTeamList()
        {
            if (_isGetTeamsRunning)
            {
                await Task.Delay(3000);
            }

            if (_teamList.Count == 0)
            {
                await GetTeams();
            }

            return _teamList;
        }

        public async Task<string> GetSeasonYearApiData()
        {
            if (_isGetSeasonParametersRunning)
            {
                await Task.Delay(3000);
            }

            if (_seasonYearApiData == null)
            {
                await GetSeasonYearParameters();
            }

            return _seasonYearApiData;
        }

        public async Task<string> GetSeasonRangeApiStat()
        {
            if (_isGetSeasonParametersRunning)
            {
                await Task.Delay(3000);
            }

            if (_seasonYearApiData == null)
            {
                await GetSeasonYearParameters();
            }

            return _seasonApiStats;
        }

        private async void GetData()
        {
            try
            {
                await GetSeasonYearParameters();
                await GetNbaPlayers();
                await GetTeams();
            }
            catch (Exception)
            {

            }


        }

        private async Task GetNbaPlayers()
        {
            try
            {
                if (_playerList.Count == 0)
                {
                    _isGetPlayersRunning = true;

                    Players players = await _nbaApiService.GetNbaPlayers(_seasonYearApiData);

                    _playerList = new List<Player>(players.League.Standard);

                    _isGetPlayersRunning = false;
                }
            }
            catch (Exception)
            {

            }
        }



        private async Task GetSeasonYearParameters()
        {
            try
            {
                if (_seasonYearApiData == null || _seasonApiStats == null)
                {
                    _isGetSeasonParametersRunning = true;
                    SeasonRange seasonRange = await _nbaApiService.GetSeasonRange();

                    DateTime seasonStartDate = DateTime.ParseExact(seasonRange.StartDateCurrentSeason, "yyyyMMdd", CultureInfo.InvariantCulture);
                    _seasonYearApiData = seasonStartDate.Year.ToString();
                    _seasonApiStats = $"{_seasonYearApiData}-{seasonStartDate.AddYears(1).ToString("yy")}";

                    _isGetSeasonParametersRunning = false;
                }
                
            }
            catch (Exception)
            {

            }

        }

        private async Task GetTeams()
        {
            try
            {
                if (_teamList.Count == 0)
                {
                    _isGetTeamsRunning = true;
                    Teams teams = await _nbaApiService.GetTeams(_seasonYearApiData);

                    _teamList = new List<Team>(teams.League.Standard);
                    _isGetTeamsRunning = false;
                }

            }
            catch (Exception)
            {

            };
        }
    }
}

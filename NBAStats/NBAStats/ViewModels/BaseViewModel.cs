﻿using NBAStats.Models;
using NBAStats.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace NBAStats.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected string _seasonYearApiData = null;
        protected string _seasonApiStats = null;
        protected List<Player> _playerList = new List<Player>();
        protected List<Team> _teamList = new List<Team>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected INbaApiService NbaApiService { get;}
        protected INavigationService NavigationService { get; }
        protected INbaDefaultInfoService NbaDefaultInfoService { get; }
        protected BaseViewModel(INavigationService navigationService, INbaApiService nbaApiService, INbaDefaultInfoService nbaDefaultInfoService)
        {
            NbaApiService = nbaApiService;
            NavigationService = navigationService;
            NbaDefaultInfoService = nbaDefaultInfoService;

        }

        public async Task GetDefaultData()
        {
            try
            {
                _seasonApiStats = await NbaDefaultInfoService.GetSeasonRangeApiStat();
                _seasonYearApiData = await NbaDefaultInfoService.GetSeasonYearApiData();
                _playerList = await NbaDefaultInfoService.GetPlayerList();
                _teamList = await NbaDefaultInfoService.GetTeamList();
            }
            catch (NoInternetConnectionException ex)
            {

            }

        }
    }
}

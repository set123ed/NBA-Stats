﻿using NBAStats.Models;
using NBAStats.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NBAStats.ViewModels
{
    public class PlayersListViewModel: BaseViewModel
    {

        public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();

        public PlayersListViewModel(INbaApiService nbaApiService, INavigationService navigationService, INbaDefaultInfoService nbaDefaultInfoService,IDatabaseService baseServices) : base(navigationService, nbaApiService, nbaDefaultInfoService,baseServices)
        {
            getdata();
        }
        public async void getdata()
        {
            await GetNamePlayers();
        }
        public async Task GetNamePlayers()
        {
            Players dateInfo = await NbaApiService.GetNbaPlayers("2020");
            Players = new ObservableCollection<Player>(dateInfo.League.Standard);
            
        }

    }
}

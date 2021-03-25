﻿using NBAStats.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using NBAStats.Models.CoachModels;
using Xamarin.Essentials;
using static NBAStats.Models.CoachModels.Coach;
using System.Windows.Input;
using Xamarin.Forms;

namespace NBAStats.ViewModels
{
    class CoachViewModel : BaseViewModel
    {
<<<<<<< HEAD
        public ObservableCollection<Standard> CoachList { get; set; }
        public NbaApiService coachApiService = new NbaApiService();
        public CoachViewModel(INbaApiService nbaApiService) : base(nbaApiService)
=======
        public CoachViewModel(INbaApiService nbaApiServices, INavigationService navigationService) : base(navigationService, nbaApiServices)
>>>>>>> bb84f7f9b3f497fcaf878d88b4310e9e344b9433
        {
            GetCoachData();
        }
        public async void GetCoachData()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var coachInfo = await coachApiService.GetCoachList();
                //await App.Current.MainPage.DisplayAlert("Coach League 2020", "There's the Coach List of 2020", "Ok");
                CoachList = new ObservableCollection<Standard>(coachInfo.League.Standard);
            }
            else
            {
                App.Current.MainPage.DisplayAlert("No Network", "Please connect to network", "Ok");
            }
        }
    }
}


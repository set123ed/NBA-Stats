using NBAStats.Models.CoachModels;
using NBAStats.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using NBAStats.Models.CoachModels;
using static NBAStats.Models.CoachModels.Coach;
using Xamarin.Essentials;


namespace NBAStats.ViewModels
{
    class CoachViewModel : BaseViewModel
    {
        public ObservableCollection<Standard> CoachList { get; set; }
        public NbaApiService coachApiService = new NbaApiService();

        public CoachViewModel(INbaApiService nbaApiServices, INavigationService navigationService) : base(navigationService, nbaApiServices)
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
                await App.Current.MainPage.DisplayAlert("No Network", "Please connect to network", "Ok");
            }

        }
    }
}

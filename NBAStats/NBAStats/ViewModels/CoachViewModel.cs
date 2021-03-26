using NBAStats.Models.CoachModels;
using NBAStats.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using static NBAStats.Models.CoachModels.Coach;
using Xamarin.Essentials;
using Prism.Services;

namespace NBAStats.ViewModels
{
    class CoachViewModel : BaseViewModel
    {
        public ObservableCollection<Standard> CoachList { get; set; }
        private IPageDialogService AlertService { get; }
        public bool Internet { get; set; } = true;

        public CoachViewModel(IPageDialogService alertService, INbaApiService nbaApiServices, INavigationService navigationService) : base(navigationService, nbaApiServices)
        {
            AlertService = alertService;
            GetCoachData();
        }
        public async void GetCoachData()
        {

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var coachInfo = await NbaApiService.GetCoachList();
                CoachList = new ObservableCollection<Standard>(coachInfo.League.Standard);
                Internet = true;


            }
            else
            {
                Internet = false;
                await AlertService.DisplayAlertAsync("No Network", "Please connect to network", "Ok");
            }

        }
    }
}

using NBAStats.Models.TeamsModels;
using NBAStats.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace NBAStats.ViewModels
{
    public class TeamViewModel : BaseViewModel
    {
        public ObservableCollection<String> Teams { get; } = new ObservableCollection<String>();
        public String Selected { get; set; }

        public bool IsBusy { get; set; }
        public bool IsNotBusy => !IsBusy;

        public TeamViewModel(INbaApiService nbaApiServices, INavigationService navigationService) : base(navigationService, nbaApiServices)
        {
            LoadTeams();
        }

        public async Task LoadTeams()
        {
            IsBusy = true;

            var teamsinformation = await NbaApiService.GetTeamsInformation();
            var Info = teamsinformation.League.Standard;

            if ((Connectivity.NetworkAccess == NetworkAccess.Internet))
            {
                if (teamsinformation != null)
                {

                    foreach (Standard team in Info)
                    {

                        Teams.Add(team.FullName.ToString());

                    }
                    Selected = Teams[0];
                }
                IsBusy = false;

            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "No tiene Acceso a internet", "cancel");
            }


        }

    }
}

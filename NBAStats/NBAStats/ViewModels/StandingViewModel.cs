using NBAStats.Models;
using NBAStats.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NBAStats.ViewModels
{
    public class StandingViewModel : BaseViewModel
    {
        public ObservableCollection<StandingPerConference> StandingPerConference { get; set; } = new ObservableCollection<StandingPerConference>();
        public ObservableCollection<TeamStanding> StandingAllLeague { get; set; } = new ObservableCollection<TeamStanding>();
        public bool ShowConference { get; set; } = true;
        public bool ShowAllLeague => !ShowConference;
        public ICommand ShowAllLeagueCommand { get; }
        public ICommand ShowConferenceCommand { get; }
        public ICommand SelectedTeamCommand { get; }

        public string SeasonStage { get; set; }

        public bool IsBusy { get; set; } = true;
        public bool IsNotBusy => !IsBusy;

        public StandingViewModel(INbaApiService nbaApiService, INavigationService navigationService, INbaDefaultInfoService nbaDefaultInfoService) : base(navigationService, nbaApiService, nbaDefaultInfoService)
        {
            ShowAllLeagueCommand = new Command(OnShowAllLeague);
            ShowConferenceCommand = new Command(OnShowConference);
            SelectedTeamCommand = new Command<string>(OnSelectedTeam);

            GetStandingData();
        }

        private async void OnSelectedTeam(string teamId)
        {
            Team teamSelected = _teamList.First(team => team.TeamId == teamId);

            var parameters = new NavigationParameters();
            parameters.Add(ParametersConstants.Team, teamSelected);

            await NavigationService.NavigateAsync(NavigationConstants.TeamProfilePage, parameters);
        }

        private void OnShowConference()
        {
            if (!ShowConference)
            {
                ShowConference = true;
            }
        }

        private void OnShowAllLeague()
        {
            if (ShowConference)
            {
                ShowConference = false;
            }
        }

        private async void GetStandingData()
        {
            await GetDefaultData();
            await GetStanding();
            IsBusy = false;
        }

        private async Task GetStanding()
        {
            try
            {
                Standing standing = await NbaApiService.GetStanding();


                if (standing != null)
                {
                    ObservableCollection<TeamStanding> standingList = standing.League.Standard.Teams;

                    SeasonStage = Config.SeasonStages.First(season => season.Id == standing.League.Standard.SeasonStageId).Stage;

                    StandingPerConference eastConference = new StandingPerConference("East");
                    StandingPerConference westConference = new StandingPerConference("West");// string constanst

                    int cont = 1;

                    foreach (TeamStanding teamStanding in standingList)
                    {
                        Team team = _teamList.First(t => t.TeamId == teamStanding.TeamId);

                        teamStanding.FullName = team.FullName;

                        if (team.ConfName == "East")
                        {
                            eastConference.Add(teamStanding);
                        }
                        else if (team.ConfName == "West")
                        {
                            westConference.Add(teamStanding);
                        }

                        teamStanding.Rank = cont++;

                        StandingAllLeague.Add(teamStanding);
                    }

                    StandingPerConference.Add(westConference);
                    StandingPerConference.Add(eastConference);
                }


            }
            catch (NoInternetConnectionException ex)
            {

                
            }
        }

    }
}

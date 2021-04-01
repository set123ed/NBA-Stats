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
    public class StandingViewModel : BaseViewModel, IInitialize
    {
        private List<Team> teamList = new List<Team>();
        public ObservableCollection<StandingPerConference> StandingPerConference { get; set; } = new ObservableCollection<StandingPerConference>();
        public ObservableCollection<TeamStanding> StandingAllLeague { get; set; } = new ObservableCollection<TeamStanding>();
        public bool ShowConference { get; set; } = true;
        public bool ShowAllLeague => !ShowConference;
        public ICommand ShowAllLeagueCommand { get; }
        public ICommand ShowConferenceCommand { get; }

        public string SeasonStage { get; set; }
        public StandingViewModel(INavigationService navigationService, INbaApiService nbaApiService) : base(navigationService, nbaApiService)
        {
            GetData();

            ShowAllLeagueCommand = new Command(OnShowAllLeague);
            ShowConferenceCommand = new Command(OnShowConference);
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

        private async void GetData()
        {
            await GetTeams();
            await GetStanding();
        }

        private async Task GetStanding()
        {
            var standingApi = await NbaApiService.GetStanding();

            if (standingApi.GetType().Name == "Standing")
            {
                if (standingApi != null)
                {
                    ObservableCollection<TeamStanding> standings = standingApi.League.Standard.Teams;

                    SeasonStage = Config.SeasonStages.First(season => season.Id == standingApi.League.Standard.SeasonStageId).Stage;

                    StandingPerConference eastConference = new StandingPerConference("East");
                    StandingPerConference westConference = new StandingPerConference("West");

                    int cont = 1;

                    foreach (TeamStanding teamStanding in standings)
                    {
                        Team team = teamList.First(t => t.TeamId == teamStanding.TeamId);

                        teamStanding.FullName = team.FullName;
                        teamStanding.L10 = $"{teamStanding.LastTenWin} - {teamStanding.LastTenLoss}";
                        teamStanding.Home = $"{teamStanding.HomeWin} - {teamStanding.HomeLoss}";
                        teamStanding.Road = $"{teamStanding.AwayWin} - {teamStanding.AwayLoss}";

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
        }

        public async Task GetTeams()
        {

            var teams = await NbaApiService.GetTeams();

            if (teams.GetType().Name == "Teams")
            {
                if (teams != null)
                {
                    teamList = teams.League.Standard;
                }
            }
        }

        public void Initialize(INavigationParameters parameters)
        {
            
        }
    }
}

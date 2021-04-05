using NBAStats.Constants;
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
        private List<Player> playerList = new List<Player>();
        public ObservableCollection<StandingPerConference> StandingPerConference { get; set; } = new ObservableCollection<StandingPerConference>();
        public ObservableCollection<TeamStanding> StandingAllLeague { get; set; } = new ObservableCollection<TeamStanding>();
        public bool ShowConference { get; set; } = true;
        public bool ShowAllLeague => !ShowConference;
        public ICommand ShowAllLeagueCommand { get; }
        public ICommand ShowConferenceCommand { get; }
        public ICommand SelectedTeamCommand { get; }

        public string SeasonStage { get; set; }
        public StandingViewModel(INavigationService navigationService, INbaApiService nbaApiService) : base(navigationService, nbaApiService)
        {
            GetData();

            ShowAllLeagueCommand = new Command(OnShowAllLeague);
            ShowConferenceCommand = new Command(OnShowConference);
            SelectedTeamCommand = new Command<string>(OnSelectedTeam);
        }

        private async void OnSelectedTeam(string teamId)
        {
            Team teamSelected = teamList.First(team => team.TeamId == teamId);

            var parameters = new NavigationParameters();
            parameters.Add(ParametersConstants.Team, teamSelected);
            parameters.Add(ParametersConstants.PlayerList, new List<Player>(playerList));
            parameters.Add(ParametersConstants.TeamList, new List<Team>(teamList));

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

        private async void GetData()
        {
            await GetTeams();
            await GetPlayers();
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
            if (teamList.Count == 0)
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

        }

        private async Task GetPlayers()
        {
            if (playerList.Count == 0)
            {
                var players = await NbaApiService.GetNbaPlayers();

                if (players.GetType().Name == "Players")
                {
                    if (players != null)
                    {

                        playerList = new List<Player>(players.League.Standard.Where(player => player.IsActive));
                    }
                }
            }
        }

        public void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(ParametersConstants.TeamList, out List<Team> teams) && parameters.TryGetValue(ParametersConstants.PlayerList, out List<Player> players))
            {
                teamList = teams;
                playerList = players;
            }
        }
    }
}

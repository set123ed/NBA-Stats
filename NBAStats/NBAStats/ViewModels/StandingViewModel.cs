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
        private List<Team> _teamList = new List<Team>();
        private List<Player> _playerList = new List<Player>();
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

        public StandingViewModel(INavigationService navigationService, INbaApiService nbaApiService) : base(navigationService, nbaApiService)
        {
            ShowAllLeagueCommand = new Command(OnShowAllLeague);
            ShowConferenceCommand = new Command(OnShowConference);
            SelectedTeamCommand = new Command<string>(OnSelectedTeam);
        }

        private async void OnSelectedTeam(string teamId)
        {
            Team teamSelected = _teamList.First(team => team.TeamId == teamId);

            var parameters = new NavigationParameters();
            parameters.Add(ParametersConstants.Team, teamSelected);
            parameters.Add(ParametersConstants.PlayerList, new List<Player>(_playerList));
            parameters.Add(ParametersConstants.TeamList, new List<Team>(_teamList));

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
            await GetSeasonYearParameters();
            await GetTeams();
            await GetPlayers();
            await GetStanding();
            IsBusy = false;
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
        }

        public async Task GetTeams()
        {
            if (_teamList.Count == 0)
            {
                var teams = await NbaApiService.GetTeams(_seasonYearApiData);

                if (teams.GetType().Name == "Teams")
                {
                    if (teams != null)
                    {
                        _teamList = teams.League.Standard;
                    }
                }
            }

        }

        private async Task GetPlayers()
        {
            if (_playerList.Count == 0)
            {
                var players = await NbaApiService.GetNbaPlayers(_seasonYearApiData);

                if (players.GetType().Name == "Players")
                {
                    if (players != null)
                    {

                        _playerList = new List<Player>(players.League.Standard.Where(player => player.IsActive));
                    }
                }
            }
        }

        public void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(ParametersConstants.TeamList, out List<Team> teamsList) && parameters.TryGetValue(ParametersConstants.PlayerList, out List<Player> playersList))
            {
                _teamList = teamsList;
                _playerList = playersList;

                
            }

            GetData();
        }
    }
}

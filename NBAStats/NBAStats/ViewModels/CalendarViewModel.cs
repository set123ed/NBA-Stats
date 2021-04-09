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
    class CalendarViewModel : BaseViewModel, IInitialize
    {
        public ObservableCollection<Game> GamesOfTheDate { get; set; } = new ObservableCollection<Game>();
        private List<Player> _playersList = new List<Player>();
        private List<Team> _teamList = new List<Team>();
        public DateTime DateSelected { get; set; } = DateTime.Today;
        private string dateFormatted => DateSelected.ToString("yyyyMMdd");
        public ICommand DateSelectedChangeCommand { get; }
        public ICommand OneDayLessCommand {get;}
        public ICommand OneDayMoreCommand {get;}
        public ICommand RefreshGamesCommand { get; }
        public ICommand GameSelectedCommand { get; }
        public bool AreGamesRefreshing { get; set; }

        public bool IsBusy { get; set; } = true;
        public bool IsNotBusy => !IsBusy;
        public CalendarViewModel(INbaApiService nbaApiServices, INavigationService navigationService) : base(navigationService, nbaApiServices)
        {
            DateSelectedChangeCommand = new Command(OnDateSelectedChange);
            RefreshGamesCommand = new Command(OnRefreshGames);

            OneDayLessCommand = new Command(OneDayLess);
            OneDayMoreCommand = new Command(OneDayMore);

            GameSelectedCommand = new Command<Game>(OnGameSelected);
        }

        public void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(ParametersConstants.TeamList, out List<Team> teamsList) && parameters.TryGetValue(ParametersConstants.PlayerList, out List<Player> playersList))
            {
                _teamList = teamsList;
                _playersList = new List<Player>(playersList.Where(player => player.IsActive)); ;
            }

            GetData();
        }

        private async void GetData()
        {
            await GetSeasonYearParameters();
            await GetNbaTeams();
            await GetPlayers();
            await GetGamesOfTheDate();
            IsBusy = false;
        }

        private async void OnGameSelected(Game game)
        {
            var parameters = new NavigationParameters();
            parameters.Add(ParametersConstants.GameId, game.GameId);
            parameters.Add(ParametersConstants.DateGame, game.StartDateEastern);
            parameters.Add(ParametersConstants.PlayerList, _playersList);
            parameters.Add(ParametersConstants.TeamList, _teamList);

            await NavigationService.NavigateAsync(NavigationConstants.BoxScorePage, parameters);
        }

        private void OneDayMore()
        {
            DateSelected = DateSelected.AddDays(1);
        }

        private void OneDayLess()
        {
            DateSelected = DateSelected.AddDays(-1);
        }

        private async void OnRefreshGames()
        {
            AreGamesRefreshing = true;
            await GetGamesOfTheDate();
            AreGamesRefreshing = false;
        }

        private async void OnDateSelectedChange()
        {
            await GetGamesOfTheDate();
        }

        public async Task GetGamesOfTheDate()
        {
            var gameOfDay = await NbaApiService.GetGamesOfDay(dateFormatted);
            if (gameOfDay.GetType().Name == "GameOfDay")
            {
                if (gameOfDay != null)
                {
                    foreach (Game game in gameOfDay.Games)
                    {
                        if (!string.IsNullOrEmpty(game.HTeam.Score) && $"{game.VTeam.Score} - {game.HTeam.Score}" != "0 - 0")
                        {

                            game.ScoreOrTime = $"{game.VTeam.Score} - {game.HTeam.Score}";

                            if (game.Period.IsHalftime)
                            {
                                game.TimePeriodHalftime = "HALFTIME";
                            }
                            else if (game.Period.IsEndOfPeriod && game.Period.CurrentPeriod <= 4)
                            {
                                game.TimePeriodHalftime = $"END OF {game.Period.CurrentPeriod} QUARTER";
                            }
                            else if (game.Period.IsEndOfPeriod && game.Period.CurrentPeriod > 4)
                            {
                                game.TimePeriodHalftime = $"END OF {game.Period.CurrentPeriod - 4} OT";
                            }
                            else if (!game.IsGameActivated)
                            {
                                game.TimePeriodHalftime = "FINAL";
                            }
                            else if (game.Period.CurrentPeriod <= 4)
                            {
                                game.TimePeriodHalftime = $"{game.Period.CurrentPeriod} QUARTER - {game.Clock} LEFT";
                            }
                            else if (game.Period.CurrentPeriod > 4)
                            {
                                game.TimePeriodHalftime = $"{game.Period.CurrentPeriod - 4} OT - {game.Clock} LEFT";
                            }
                        }
                        else
                        {
                            game.ScoreOrTime = game.StartTimeEastern;
                        }
                    }

                    GamesOfTheDate= new ObservableCollection<Game>(gameOfDay.Games);

                }

            }

        }

        private async Task GetNbaTeams()
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
            if (_playersList.Count == 0)
            {
                var players = await NbaApiService.GetNbaPlayers(_seasonYearApiData);

                if (players.GetType().Name == "Players")
                {
                    if (players != null)
                    {

                        _playersList = new List<Player>(players.League.Standard.Where(player => player.IsActive));
                    }
                }
            }
        }
    }
}

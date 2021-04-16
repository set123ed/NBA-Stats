using NBAStats.Models;
using NBAStats.Services;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
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
    public class HomeViewModel : BaseViewModel
    {
        int _position;
        public int GamePosition { get; set; }

        private int _gamesOfTodayCount => GamesOfDay.Count;

        private string _todayDate = DateTime.Today.ToString("yyyyMMdd");
        public ObservableCollection<Game> GamesOfDay { get; set; } = new ObservableCollection<Game>();
        public ObservableCollection<PlayerRegularStats> ScoringLeaders { get; set; }
        public ObservableCollection<BetterTeams> BetterTeams { get; set; }
        public bool IsGamesOfDayRefreshing { get; set; }
        public ICommand RefreshGamesOfDayCommand { get; }
        public ICommand GameSelectedCommand { get; }
        public ICommand PlayerSelectedCommand { get; }
        public ICommand TeamSelectedCommand { get; set; }
        public ICommand ListPlayers { get; }
        public IPageDialogService AlertService { get; }

        private bool _areGamesBeingPlayed = false;
        private List<string> _gamesBeingPlayed = new List<string>();

        public bool IsBusy { get; set; } = true;
        public bool IsNotBusy => !IsBusy;


        public HomeViewModel(IPageDialogService dialogService,INbaApiService nbaApiService, INavigationService navigationService, INbaDefaultInfoService nbaDefaultInfoService, IDatabaseService baseServices) : base(navigationService, nbaApiService, nbaDefaultInfoService,baseServices)
        {
            AlertService = dialogService;

           


       

            TeamSelectedCommand = new Command<string>(OnSelectedTeam);
            RefreshGamesOfDayCommand = new Command(OnRefreshGamesOfDay);
            GameSelectedCommand = new Command<Game>(OnGameSelected);
            PlayerSelectedCommand = new Command<PlayerRegularStats>(OnPlayerSelected);

            ListPlayers = new Command(PlayerList);

            GetHomeData();
        }

        private async void PlayerList()
        {
            await NavigationService.NavigateAsync(NavigationConstants.PlayersList);
        }

        private async void OnPlayerSelected(PlayerRegularStats player)
        {
            string playerId = "";

            if (player.PlayerId.Contains(" "))
            {
                string firstPlayer = player.Name.Substring(0, player.Name.IndexOf('-')).Trim();
                string secondPlayer = player.Name.Substring(player.Name.LastIndexOf('-') + 1).Trim();
                bool playedSelected = false;

                var action = await AlertService.DisplayActionSheetAsync(StringConstants.ChoosePlayer, StringConstants.Cancel, null, firstPlayer, secondPlayer);

                if (action == firstPlayer)
                {
                    playerId = player.PlayerId.Substring(0, player.PlayerId.IndexOf(' ')).Trim();
                    playedSelected = true;
                }
                else if (action == secondPlayer)
                {
                    playerId = player.PlayerId.Substring(player.PlayerId.IndexOf(' ')).Trim();
                    playedSelected = true;
                }

                if (playedSelected)
                {
                    var parameters = new NavigationParameters();
                    parameters.Add(ParametersConstants.PlayerId, playerId);

                    await NavigationService.NavigateAsync(NavigationConstants.PlayerProfilePage, parameters);
                }
            }
            else
            {
                playerId = player.PlayerId;
                var parameters = new NavigationParameters();
                parameters.Add(ParametersConstants.PlayerId, playerId);

                await NavigationService.NavigateAsync(NavigationConstants.PlayerProfilePage, parameters);
            }

        }

        private async void OnSelectedTeam(string teamId)
        {
            Team teamSelected = _teamList.First(team => team.TeamId == teamId);

            var parameters = new NavigationParameters();
            parameters.Add(ParametersConstants.Team, teamSelected);

            await NavigationService.NavigateAsync(NavigationConstants.TeamProfilePage, parameters);
        }

        private async void OnGameSelected(Game game)
        {
            var parameters = new NavigationParameters();
            parameters.Add(ParametersConstants.GameId, game.GameId);
            parameters.Add(ParametersConstants.DateGame, game.StartDateEastern);

            await NavigationService.NavigateAsync(NavigationConstants.BoxScorePage, parameters);
        }

        private void SwipeGamesAutomatic()
        {
            Device.StartTimer(TimeSpan.FromSeconds(5), (Func<bool>)(() =>
            {
                GamePosition = (GamePosition + 1) % _gamesOfTodayCount;
                return true;
            }));
        }

        private async void GetHomeData()
        {
            await GetDefaultData();
            await GetGamesOfTheDay();
            await GetScoringLeaders();
            await GetBetterTeams();
            IsBusy = false;
            SwipeGamesAutomatic();
        }

        private async void OnRefreshGamesOfDay()
        {
            IsGamesOfDayRefreshing = true;
            await GetGamesOfTheDay();
            await GetScoringLeaders();
            await GetBetterTeams();
            IsGamesOfDayRefreshing = false;
        }

        private async Task GetGamesOfTheDay()
        {
            try
            {
                GameOfDay gameOfDay = await NbaApiService.GetGamesOfDay(_todayDate);
                if (gameOfDay != null)
                {
                    foreach (Game game in gameOfDay.Games)
                    {
                        game.ScoreOrTime = Utilities.GetScoreOrTime(game.VTeam.Score, game.HTeam.Score, game.StartTimeEastern);
                        game.TimePeriodHalftime = Utilities.GetTimePeriod(game.VTeam.Score, game.HTeam.Score, game.Period.CurrentPeriod, game.Period.IsHalftime, game.Period.IsEndOfPeriod, game.IsGameActivated, game.Clock);

                        if (game.ScoreOrTime.Contains("-"))
                        {
                            _areGamesBeingPlayed = true;
                            _gamesBeingPlayed.Add(game.GameId);
                        }

                    }

                    GamesOfDay = new ObservableCollection<Game>(gameOfDay.Games);
                }
            }
            catch (NoInternetConnectionException ex)
            {
                await AlertService.DisplayAlertAsync("Error", ex.Message, "ok");
            }

        }

        private async Task GetScoringLeaders()
        {
            try
            {
                if (_areGamesBeingPlayed)
                {
                    ObservableCollection<PlayerRegularStats> playerScoringLeaderList = new ObservableCollection<PlayerRegularStats>();

                    foreach (string gameId in _gamesBeingPlayed)
                    {
                        BoxScore boxScore = await NbaApiService.GetBoxScore(_todayDate, gameId);

                        if (boxScore != null)
                        {

                            PlayerRegularStats playerRegularStats = new PlayerRegularStats();

                            if (Convert.ToDecimal(boxScore.Stats.VTeam.Leaders.Points.Value) > Convert.ToDecimal(boxScore.Stats.HTeam.Leaders.Points.Value))
                            {
                                playerRegularStats.Name = boxScore.Stats.VTeam.Leaders.Points.Players[0].FirstName + " " + boxScore.Stats.VTeam.Leaders.Points.Players[0].LastName;
                                playerRegularStats.Team = boxScore.BasicGameData.VTeam.TriCode;
                                playerRegularStats.TeamId = boxScore.BasicGameData.VTeam.TeamId;
                                playerRegularStats.PlayerId = boxScore.Stats.VTeam.Leaders.Points.Players[0].PersonId;
                                playerRegularStats.PointsPerGame = boxScore.Stats.VTeam.Leaders.Points.Value;
                                playerRegularStats.AssistsPerGame = boxScore.Stats.ActivePlayers.First(player => player.PersonId == playerRegularStats.PlayerId).Assists;
                                playerRegularStats.ReboundsPerGame = boxScore.Stats.ActivePlayers.First(player => player.PersonId == playerRegularStats.PlayerId).TotReb;
                            }
                            else if (Convert.ToDecimal(boxScore.Stats.VTeam.Leaders.Points.Value) < Convert.ToDecimal(boxScore.Stats.HTeam.Leaders.Points.Value))
                            {
                                playerRegularStats.Name = boxScore.Stats.HTeam.Leaders.Points.Players[0].FirstName + " " + boxScore.Stats.HTeam.Leaders.Points.Players[0].LastName;
                                playerRegularStats.Team = boxScore.BasicGameData.HTeam.TriCode;
                                playerRegularStats.TeamId = boxScore.BasicGameData.HTeam.TeamId;
                                playerRegularStats.PlayerId = boxScore.Stats.HTeam.Leaders.Points.Players[0].PersonId;
                                playerRegularStats.PointsPerGame = boxScore.Stats.HTeam.Leaders.Points.Value;
                                playerRegularStats.AssistsPerGame = boxScore.Stats.ActivePlayers.First(player => player.PersonId == playerRegularStats.PlayerId).Assists;
                                playerRegularStats.ReboundsPerGame = boxScore.Stats.ActivePlayers.First(player => player.PersonId == playerRegularStats.PlayerId).TotReb;
                            }
                            else
                            {
                                playerRegularStats.Name = boxScore.Stats.VTeam.Leaders.Points.Players[0].FirstName + " " + boxScore.Stats.VTeam.Leaders.Points.Players[0].LastName
                                                            + " - " + boxScore.Stats.HTeam.Leaders.Points.Players[0].FirstName + " " + boxScore.Stats.HTeam.Leaders.Points.Players[0].LastName;
                                playerRegularStats.PlayerId = boxScore.Stats.VTeam.Leaders.Points.Players[0].PersonId + " " + boxScore.Stats.HTeam.Leaders.Points.Players[0].PersonId;
                                playerRegularStats.PointsPerGame = boxScore.Stats.HTeam.Leaders.Points.Value;
                                playerRegularStats.Team = boxScore.BasicGameData.VTeam.TriCode + " - " + boxScore.BasicGameData.HTeam.TriCode;
                                playerRegularStats.AssistsPerGame = boxScore.Stats.ActivePlayers.First(player => player.PersonId == boxScore.Stats.VTeam.Leaders.Points.Players[0].PersonId).Assists + " - " +
                                        boxScore.Stats.ActivePlayers.First(player => player.PersonId == boxScore.Stats.HTeam.Leaders.Points.Players[0].PersonId).Assists;
                                playerRegularStats.ReboundsPerGame = boxScore.Stats.ActivePlayers.First(player => player.PersonId == boxScore.Stats.VTeam.Leaders.Points.Players[0].PersonId).TotReb + " - " +
                                    boxScore.Stats.ActivePlayers.First(player => player.PersonId == boxScore.Stats.HTeam.Leaders.Points.Players[0].PersonId).TotReb;
                            }

                            playerScoringLeaderList.Add(playerRegularStats);

                        }


                    }

                    ScoringLeaders = playerScoringLeaderList;
                }
                else
                {
                    PlayerStatsLeaders playerStatsLeaders = await NbaApiService.GetPlayerStatsLeaders(_seasonApiStats, StringConstants.PtsStatParameter);
                    ObservableCollection<PlayerRegularStats> listPlayerRegularStats = new ObservableCollection<PlayerRegularStats>();

                    for (int i = 0; i < 6; i++)
                    {
                        PlayerRegularStats newPlayer = new PlayerRegularStats();

                        newPlayer.PlayerId = playerStatsLeaders.ResultSet.RowSet[i][0].ToString();
                        newPlayer.Name = playerStatsLeaders.ResultSet.RowSet[i][2].ToString();
                        newPlayer.Team = playerStatsLeaders.ResultSet.RowSet[i][3].ToString();
                        newPlayer.TeamId = _teamList.First(team => team.Tricode.ToLower() == newPlayer.Team.ToLower()).TeamId;
                        newPlayer.PointsPerGame = Math.Round(Convert.ToDecimal(playerStatsLeaders.ResultSet.RowSet[i][22].ToString()), 1).ToString();
                        newPlayer.AssistsPerGame = Math.Round(Convert.ToDecimal(playerStatsLeaders.ResultSet.RowSet[i][18].ToString()), 1).ToString();
                        newPlayer.ReboundsPerGame = Math.Round(Convert.ToDecimal(playerStatsLeaders.ResultSet.RowSet[i][17].ToString()), 1).ToString();

                        listPlayerRegularStats.Add(newPlayer);
                    }

                    ScoringLeaders = listPlayerRegularStats;
                }

            }
            catch (NoInternetConnectionException ex)
            {

                
            }

        }

        private async Task GetBetterTeams()
        {
            try
            {
                Standing standing = await NbaApiService.GetStanding();
                TeamStatsClass teamStatsClass = await NbaApiService.GetTeamStats(_seasonYearApiData);

                if (standing != null && teamStatsClass != null)
                {
                    ObservableCollection<BetterTeams> betterTeams = new ObservableCollection<BetterTeams>();

                    for (int i = 0; i < 5; i++)
                    {
                        BetterTeams better = new BetterTeams();

                        better.TeamStanding = standing.League.Standard.Teams[i];
                        better.TeamStats = teamStatsClass.LeagueTeamStats.Seasons.RegularSeason.Teams.First(team => team.TeamId == better.TeamStanding.TeamId);

                        betterTeams.Add(better);
                    }

                    BetterTeams = betterTeams;

                }

            }
            catch (NoInternetConnectionException ex)
            {

            }

        }

    }
}

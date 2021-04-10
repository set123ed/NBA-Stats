 using NBAStats.Constants;
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
    public class HomeViewModel : BaseViewModel, IInitialize 
    {
        int _position;
        public int ImagePosition { get; set; }

        private string _todayDate = DateTime.Today.ToString("yyyyMMdd");
        private List<Team> _teamList = new List<Team>();
        public ObservableCollection<Game> GamesOfDay { get; set; } = new ObservableCollection<Game>();
        public ObservableCollection<PlayerRegularStats> ScoringLeaders { get; set; }
        public ObservableCollection<BetterTeams> BetterTeams { get; set; }
        public bool IsGamesOfDayRefreshing { get; set; }
        public ICommand RefreshGamesOfDayCommand { get; }
        public ICommand GameSelectedCommand { get; }
        public ICommand PlayerSelectedCommand { get; }
        public ICommand TeamSelectedCommand { get; set; }
        public IPageDialogService AlertService { get; }

        private bool _areGamesBeingPlayed = false;
        private List<string> _gamesBeingPlayed = new List<string>();
        private List<Player> _playersList = new List<Player>();

        public bool IsBusy { get; set; } = true;
        public bool IsNotBusy => !IsBusy;


        public HomeViewModel(IPageDialogService dialogService, INbaApiService nbaApiService, INavigationService navigationService) : base(navigationService, nbaApiService)
        {
            AlertService = dialogService;

           

            Device.StartTimer(TimeSpan.FromSeconds(3), (Func<bool>)(() =>
            {
                 ImagePosition = (ImagePosition + 1) % 5;
                 return true;
            }));
       

            TeamSelectedCommand = new Command<string>(OnSelectedTeam);
            RefreshGamesOfDayCommand = new Command(OnRefreshGamesOfDay);
            GameSelectedCommand = new Command<Game>(OnGameSelected);
            PlayerSelectedCommand = new Command<PlayerRegularStats>(OnPlayerSelected);
        }

        private async void OnPlayerSelected(PlayerRegularStats player)
        {
            string playerId = "";

            if (player.PlayerId.Contains(" "))
            {
                string firstPlayer = player.Name.Substring(0, player.Name.IndexOf('-')).Trim();
                string secondPlayer = player.Name.Substring(player.Name.LastIndexOf('-') + 1).Trim();
                bool playedSelected = false;

                var action = await AlertService.DisplayActionSheetAsync("Choose a player", "Cancel", null, firstPlayer, secondPlayer);

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
                    parameters.Add(ParametersConstants.PlayerList, _playersList);
                    parameters.Add(ParametersConstants.TeamList, _teamList);

                    await NavigationService.NavigateAsync(NavigationConstants.PlayerProfilePage, parameters);
                }
            }
            else
            {
                playerId = player.PlayerId;
                var parameters = new NavigationParameters();
                parameters.Add(ParametersConstants.PlayerId, playerId);
                parameters.Add(ParametersConstants.PlayerList, _playersList);
                parameters.Add(ParametersConstants.TeamList, _teamList);

                await NavigationService.NavigateAsync(NavigationConstants.PlayerProfilePage, parameters);
            }

        }

        private async void OnSelectedTeam(string teamId)
        {
            Team teamSelected = _teamList.First(team => team.TeamId == teamId);

            var parameters = new NavigationParameters();
            parameters.Add(ParametersConstants.Team, teamSelected);
            parameters.Add(ParametersConstants.PlayerList, _playersList);
            parameters.Add(ParametersConstants.TeamList, _teamList);

            await NavigationService.NavigateAsync(NavigationConstants.TeamProfilePage, parameters);
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

        private async void GetData()
        {
            await GetSeasonYearParameters();
            await GetGamesOfTheDay();
            await GetScoringLeaders();
            await GetBetterTeams();
            await GetPlayers();
            await GetNbaTeams();
            IsBusy = false;
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
            GameOfDay gameOfDay = await NbaApiService.GetGamesOfDay(_todayDate);
            if (gameOfDay != null)
            {
                foreach (Game game in gameOfDay.Games)
                {
                    if (!string.IsNullOrEmpty(game.HTeam.Score) && $"{game.VTeam.Score} - {game.HTeam.Score}" != "0 - 0")
                    {
                        _areGamesBeingPlayed = true;
                        _gamesBeingPlayed.Add(game.GameId);

                        game.ScoreOrTime = $"{game.VTeam.Score} - {game.HTeam.Score}";

                        if (game.Period.IsHalftime)
                        {
                            game.TimePeriodHalftime = "HALFTIME";
                        }
                        else if (game.Period.IsEndOfPeriod && game.Period.CurrentPeriod <= 4)
                        {
                            game.TimePeriodHalftime = $"END OF {game.Period.CurrentPeriod} PERIOD";
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
                            game.TimePeriodHalftime = $"{game.Period.CurrentPeriod} PERIOD - {game.Clock} LEFT";
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

                GamesOfDay = new ObservableCollection<Game>(gameOfDay.Games);

            }
        }

        private async Task GetScoringLeaders()
        {
            if (_areGamesBeingPlayed)
            {
                ObservableCollection<PlayerRegularStats> playerScoringLeaderList = new ObservableCollection<PlayerRegularStats>();

                foreach (string gameId in _gamesBeingPlayed)
                {
                    var boxScore = await NbaApiService.GetBoxScore(_todayDate, gameId);

                    if (boxScore.GetType().Name == "BoxScore")
                    {
                        if (boxScore != null)
                        {


                            PlayerRegularStats playerRegularStats = new PlayerRegularStats();

                            if (Convert.ToDecimal(boxScore.Stats.VTeam.Leaders.Points.Value) > Convert.ToDecimal(boxScore.Stats.HTeam.Leaders.Points.Value))
                            {
                                playerRegularStats.Name = boxScore.Stats.VTeam.Leaders.Points.Players[0].FirstName + " " + boxScore.Stats.VTeam.Leaders.Points.Players[0].LastName;
                                playerRegularStats.Team = boxScore.BasicGameData.VTeam.TriCode;
                                playerRegularStats.PlayerId = boxScore.Stats.VTeam.Leaders.Points.Players[0].PersonId;
                                playerRegularStats.PointsPerGame = boxScore.Stats.VTeam.Leaders.Points.Value;
                                playerRegularStats.AssistsPerGame = boxScore.Stats.ActivePlayers.First(player => player.PersonId == playerRegularStats.PlayerId).Assists;
                                playerRegularStats.ReboundsPerGame = boxScore.Stats.ActivePlayers.First(player => player.PersonId == playerRegularStats.PlayerId).TotReb;
                            }
                            else if (Convert.ToDecimal(boxScore.Stats.VTeam.Leaders.Points.Value) < Convert.ToDecimal(boxScore.Stats.HTeam.Leaders.Points.Value))
                            {
                                playerRegularStats.Name = boxScore.Stats.HTeam.Leaders.Points.Players[0].FirstName + " " + boxScore.Stats.HTeam.Leaders.Points.Players[0].LastName;
                                playerRegularStats.Team = boxScore.BasicGameData.HTeam.TriCode;
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
                }

                ScoringLeaders = playerScoringLeaderList;
            }
            else
            {
                PlayerStatsLeaders playerStatsLeaders = await NbaApiService.GetPlayerStatsLeaders(_seasonApiStats,"PTS");
                ObservableCollection<PlayerRegularStats> listPlayerRegularStats = new ObservableCollection<PlayerRegularStats>();

                for (int i = 0; i < 6; i++)
                {
                    PlayerRegularStats newPlayer = new PlayerRegularStats();

                    newPlayer.PlayerId = playerStatsLeaders.ResultSet.RowSet[i][0].ToString();
                    newPlayer.Name = playerStatsLeaders.ResultSet.RowSet[i][2].ToString();
                    newPlayer.Team = playerStatsLeaders.ResultSet.RowSet[i][3].ToString();
                    newPlayer.PointsPerGame = Math.Round(Convert.ToDecimal(playerStatsLeaders.ResultSet.RowSet[i][22].ToString()), 1).ToString();
                    newPlayer.AssistsPerGame = Math.Round(Convert.ToDecimal(playerStatsLeaders.ResultSet.RowSet[i][18].ToString()), 1).ToString();
                    newPlayer.ReboundsPerGame = Math.Round(Convert.ToDecimal(playerStatsLeaders.ResultSet.RowSet[i][17].ToString()), 1).ToString();

                    listPlayerRegularStats.Add(newPlayer);
                }

                ScoringLeaders = listPlayerRegularStats;
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

        private async Task GetBetterTeams()
        {
            var standing = await NbaApiService.GetStanding();
            var teamStatsClass = await NbaApiService.GetTeamStats(_seasonYearApiData);

            if (standing.GetType().Name == "Standing" && teamStatsClass.GetType().Name == "TeamStatsClass")
            {
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

        public void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(ParametersConstants.TeamList, out List<Team> teamsList) && parameters.TryGetValue(ParametersConstants.PlayerList, out List<Player> playersList))
            {
                _teamList = teamsList;
                _playersList = playersList;
            }

            GetData();
        }
    }
}

using NBAStats.Constants;
using NBAStats.Models;
using NBAStats.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace NBAStats.ViewModels
{
    public class BoxScoreViewModel : BaseViewModel, IInitialize
    {
        private string _gameId;
        private string _dateGame;

        private List<Player> _playersList = new List<Player>();
        private List<Team> _teamList = new List<Team>();

        public ObservableCollection<ActivePlayerBoxScore> HTeamPlayerStats { get; set; }
        public ObservableCollection<ActivePlayerBoxScore> VTeamPlayerStats { get; set; }

        public string HTeamName { get; set; }
        public string VTeamName { get; set; }
        public string HTeamColor { get; set; } = "Blue";
        public string VTeamColor { get; set; } = "MidnightBlue";

        public ICommand HTeamSelectedCommand { get; }
        public ICommand VTeamSelectedCommand { get; }
        public ICommand SelectedPlayerCommand { get; }
        
        public bool ShowVTeam { get; set; }
        public bool ShowHTeam => !ShowVTeam;
        public string ScoreOrTime { get; set; }
        public string TimePeriodHalftime { get; set; }

        public bool IsBusy { get; set; } = true;
        public bool IsNotBusy => !IsBusy;

        public BoxScoreViewModel(INbaApiService nbaApiService, INavigationService navigationService) : base(navigationService,nbaApiService)
        {
            HTeamSelectedCommand = new Command(OnHTeamSelected);
            VTeamSelectedCommand = new Command(OnVTeamSelected);
            SelectedPlayerCommand = new Command<string>(OnSelectedPlayer);
        }

        private async void OnSelectedPlayer(string playerName)
        {
            ActivePlayerBoxScore playerSelected = new ActivePlayerBoxScore();

            if (ShowHTeam)
            {
                playerSelected = HTeamPlayerStats.First(player => player.FullName == playerName);
            }
            else
            {
                playerSelected = VTeamPlayerStats.First(player => player.FullName == playerName);
            }

            var parameters = new NavigationParameters();
            parameters.Add(ParametersConstants.TeamList, _teamList);
            parameters.Add(ParametersConstants.PlayerId, playerSelected.PersonId);
            parameters.Add(ParametersConstants.PlayerList, _playersList);

            await NavigationService.NavigateAsync(NavigationConstants.PlayerProfilePage, parameters);
        }

        private void OnVTeamSelected()
        {
            if (!ShowVTeam)
            {
                VTeamColor = "MidnightBlue";
                HTeamColor = "Blue";
                ShowVTeam = true;
            }
        }

        private void OnHTeamSelected()
        {
            if (!ShowHTeam)
            {
                HTeamColor = "MidnightBlue";
                VTeamColor = "Blue";
                ShowVTeam = false;
            }
        }

        public async void GetBoxScore()
        {
            await GetSeasonYearParameters();

            var boxScore = await NbaApiService.GetBoxScore(_dateGame,_gameId);

            if (boxScore.GetType().Name == "BoxScore")
            {
                if (boxScore != null)
                {

                    HTeamName = boxScore.BasicGameData.HTeam.TriCode;
                    VTeamName = boxScore.BasicGameData.VTeam.TriCode;

                    GetTime(boxScore);

                    string hTeamId = boxScore.BasicGameData.HTeam.TeamId;
                    string vTeamId = boxScore.BasicGameData.VTeam.TeamId;

                    ObservableCollection<ActivePlayerBoxScore> hTeamPlayerStats = new ObservableCollection<ActivePlayerBoxScore>();
                    ObservableCollection<ActivePlayerBoxScore> vTeamPlayerStats = new ObservableCollection<ActivePlayerBoxScore>();

                    if (boxScore.Stats != null)
                    {

                        foreach (ActivePlayerBoxScore player in boxScore.Stats.ActivePlayers)
                        {
                            player.FullName = $"{player.FirstName} {player.LastName}";

                            if (hTeamId == player.TeamId)
                            {
                                hTeamPlayerStats.Add(player);
                            }
                            else if (vTeamId == player.TeamId)
                            {
                                vTeamPlayerStats.Add(player);
                            }
                        }


                        HTeamPlayerStats = new ObservableCollection<ActivePlayerBoxScore>(hTeamPlayerStats.OrderByDescending(p => p.Min));
                        VTeamPlayerStats = new ObservableCollection<ActivePlayerBoxScore>(vTeamPlayerStats.OrderByDescending(p => p.Min));


                    }
                    else
                    {

                        foreach (Player player in _playersList)
                        {
                            ActivePlayerBoxScore playerBoxScore = new ActivePlayerBoxScore();
                            playerBoxScore.PersonId = player.PersonId;
                            playerBoxScore.FullName = player.FullName;

                            if (player.TeamId == hTeamId)
                            {
                                hTeamPlayerStats.Add(playerBoxScore);
                            }
                            else if (player.TeamId == vTeamId)
                            {
                                vTeamPlayerStats.Add(playerBoxScore);
                            }

                        }

                        HTeamPlayerStats = hTeamPlayerStats;
                        VTeamPlayerStats = vTeamPlayerStats;
                    }

                    ShowVTeam = true;

                }
            }

            IsBusy = false;
        }

        public void GetTime(BoxScore boxScore)
        {
            if (!string.IsNullOrEmpty(boxScore.BasicGameData.HTeam.Score) && $"{boxScore.BasicGameData.VTeam.Score} - {boxScore.BasicGameData.HTeam.Score}" != "0 - 0")
            {

                ScoreOrTime = $"{boxScore.BasicGameData.VTeam.Score} - {boxScore.BasicGameData.HTeam.Score}";

                if (boxScore.BasicGameData.Period.IsHalftime)
                {
                    TimePeriodHalftime = "HALFTIME";
                }
                else if (boxScore.BasicGameData.Period.IsEndOfPeriod && boxScore.BasicGameData.Period.CurrentPeriod <= 4)
                {
                    TimePeriodHalftime = $"END OF {boxScore.BasicGameData.Period.CurrentPeriod} QUARTER";
                }
                else if (boxScore.BasicGameData.Period.IsEndOfPeriod && boxScore.BasicGameData.Period.CurrentPeriod > 4)
                {
                    TimePeriodHalftime = $"END OF {boxScore.BasicGameData.Period.CurrentPeriod - 4} OT";
                }
                else if (!boxScore.BasicGameData.IsGameActivated)
                {
                    TimePeriodHalftime = "FINAL";
                }
                else if (boxScore.BasicGameData.Period.CurrentPeriod <= 4)
                {
                    TimePeriodHalftime = $"{boxScore.BasicGameData.Period.CurrentPeriod} QUARTER - {boxScore.BasicGameData.Clock} LEFT";
                }
                else if (boxScore.BasicGameData.Period.CurrentPeriod > 4)
                {
                    TimePeriodHalftime = $"{boxScore.BasicGameData.Period.CurrentPeriod - 4} OT - {boxScore.BasicGameData.Clock} LEFT";
                }
            }
            else
            {
                ScoreOrTime = boxScore.BasicGameData.StartTimeEastern;
            }
        }

        public void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(ParametersConstants.GameId, out string gameId) && parameters.TryGetValue(ParametersConstants.DateGame, out string dateGame) &&
                parameters.TryGetValue(ParametersConstants.PlayerList, out List<Player> playersList) && parameters.TryGetValue(ParametersConstants.TeamList, out List<Team> teamsList))
            {
                _gameId = gameId;
                _dateGame = dateGame;

                _playersList = playersList;
                _teamList = teamsList;
            }

            GetBoxScore();
        }
    }

}

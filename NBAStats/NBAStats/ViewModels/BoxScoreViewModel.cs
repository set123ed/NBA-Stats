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
    public class BoxScoreViewModel : BaseViewModel, IInitialize
    {
        private string _gameId;
        private string _dateGame;

        public ObservableCollection<ActivePlayerBoxScore> HTeamPlayerStats { get; set; }
        public TotalTeamStatsBoxScore HTeamTotalStats { get; set; }
        public ObservableCollection<ActivePlayerBoxScore> VTeamPlayerStats { get; set; }
        public TotalTeamStatsBoxScore VTeamTotalStats { get; set; }

        public string HTeamName { get; set; }
        public string VTeamName { get; set; }
        public string HTeamLogo { get; set; }
        public string VTeamLogo { get; set; }
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

        public BoxScoreViewModel(INbaApiService nbaApiService, INavigationService navigationService, INbaDefaultInfoService nbaDefaultInfoService) : base(navigationService,nbaApiService, nbaDefaultInfoService)
        {
            HTeamSelectedCommand = new Command(OnHTeamSelected);
            VTeamSelectedCommand = new Command(OnVTeamSelected);
            SelectedPlayerCommand = new Command<string>(OnSelectedPlayer);
        }
        //pasar id
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
            parameters.Add(ParametersConstants.PlayerId, playerSelected.PersonId);

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

        public async Task GetBoxScore()
        {
            try
            {
                BoxScore boxScore = await NbaApiService.GetBoxScore(_dateGame, _gameId);

                if (boxScore != null)
                {

                    HTeamName = boxScore.BasicGameData.HTeam.TriCode;
                    VTeamName = boxScore.BasicGameData.VTeam.TriCode;
                    HTeamLogo = boxScore.BasicGameData.HTeam.TeamLogo;
                    VTeamLogo = boxScore.BasicGameData.VTeam.TeamLogo;

                    ScoreOrTime = Utilities.GetScoreOrTime(boxScore.BasicGameData.VTeam.Score, boxScore.BasicGameData.HTeam.Score, boxScore.BasicGameData.StartTimeEastern);
                    TimePeriodHalftime = Utilities.GetTimePeriod(boxScore.BasicGameData.VTeam.Score, boxScore.BasicGameData.HTeam.Score, boxScore.BasicGameData.Period.CurrentPeriod, boxScore.BasicGameData.Period.IsHalftime, boxScore.BasicGameData.Period.IsEndOfPeriod, boxScore.BasicGameData.IsGameActivated, boxScore.BasicGameData.Clock);

                    string hTeamId = boxScore.BasicGameData.HTeam.TeamId;
                    string vTeamId = boxScore.BasicGameData.VTeam.TeamId;

                    ObservableCollection<ActivePlayerBoxScore> hTeamPlayerStats = new ObservableCollection<ActivePlayerBoxScore>();
                    ObservableCollection<ActivePlayerBoxScore> vTeamPlayerStats = new ObservableCollection<ActivePlayerBoxScore>();

                    if (boxScore.Stats != null)
                    {
                        HTeamTotalStats = boxScore.Stats.HTeam.Totals;
                        VTeamTotalStats = boxScore.Stats.VTeam.Totals;

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

                        foreach (Player player in _playerList)
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


                IsBusy = false;
            }
            catch (NoInternetConnectionException ex)
            {

            }
        }

        public async void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(ParametersConstants.GameId, out string gameId) && parameters.TryGetValue(ParametersConstants.DateGame, out string dateGame))            {
                _gameId = gameId;
                _dateGame = dateGame;
            }

            await GetDefaultData();
            await GetBoxScore();
        }
    }

}

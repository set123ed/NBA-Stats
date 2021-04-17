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

        public bool IsBoxScoreRefreshing { get; set; } = false;
        public string HTeamName { get; set; }
        public string VTeamName { get; set; }
        public string HTeamLogo { get; set; }
        public string VTeamLogo { get; set; }
        public string HTeamColor { get; set; } = "DarkRed";
        public string VTeamColor { get; set; } = "Red";

        public ICommand HTeamSelectedCommand { get; }
        public ICommand VTeamSelectedCommand { get; }
        public ICommand SelectedPlayerCommand { get; }
        public ICommand RefreshBoxScoreCommand { get; }
        public bool ShowVTeam { get; set; }
        public bool ShowHTeam => !ShowVTeam;
        public string ScoreOrTime { get; set; }
        public string TimePeriodHalftime { get; set; }

        public bool IsBusy { get; set; } = true;
        public bool IsNotBusy => !IsBusy;

        public BoxScoreViewModel(INbaApiService nbaApiService, INavigationService navigationService, INbaDefaultInfoService nbaDefaultInfoService,IDatabaseService baseServices) : base(navigationService,nbaApiService, nbaDefaultInfoService, baseServices)
        {
            HTeamSelectedCommand = new Command(OnHTeamSelected);
            VTeamSelectedCommand = new Command(OnVTeamSelected);
            SelectedPlayerCommand = new Command<ActivePlayerBoxScore>(OnSelectedPlayer);
            RefreshBoxScoreCommand = new Command(OnRefreshBoxScore);
        }
        private async void OnRefreshBoxScore()
        {
            IsBoxScoreRefreshing = true;
            if (TimePeriodHalftime != StringConstants.FinalGame)
            {
                await GetBoxScore();
            }
            IsBoxScoreRefreshing = false;
        }
        private async void OnSelectedPlayer(ActivePlayerBoxScore playerSelected)
        {
            var parameters = new NavigationParameters();
            parameters.Add(ParametersConstants.PlayerId, playerSelected.PersonId);

            await NavigationService.NavigateAsync(NavigationConstants.PlayerProfilePage, parameters);
        }

        private void OnVTeamSelected()
        {
            if (!ShowVTeam)
            {
                VTeamColor = "DarkRed";
                HTeamColor = "Red";
                ShowVTeam = true;
            }
        }

        private void OnHTeamSelected()
        {
            if (!ShowHTeam)
            {
                HTeamColor = "DarkRed";
                VTeamColor = "Red";
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


                        HTeamPlayerStats = Utilities.SetBoxScoresFavoritesPlayers(hTeamPlayerStats, AllFavoritesPlayers);
                        VTeamPlayerStats = Utilities.SetBoxScoresFavoritesPlayers(vTeamPlayerStats, AllFavoritesPlayers);


                    }
                    else
                    {
                        List<Player> hTeamRoster = new List<Player>(Utilities.SetFavoritesPlayers(_playerList.Where(p => p.TeamId == hTeamId), AllFavoritesPlayers));
                        List<Player> vTeamRoster = new List<Player>(Utilities.SetFavoritesPlayers(_playerList.Where(p => p.TeamId == vTeamId),AllFavoritesPlayers));

                        foreach (Player player in hTeamRoster)
                        {
                            ActivePlayerBoxScore playerBoxScore = new ActivePlayerBoxScore();
                            playerBoxScore.PersonId = player.PersonId;
                            playerBoxScore.FullName = player.FullName;

                            hTeamPlayerStats.Add(playerBoxScore);
                        }
                        
                        foreach (Player player in vTeamRoster)
                        {
                            ActivePlayerBoxScore playerBoxScore = new ActivePlayerBoxScore();
                            playerBoxScore.PersonId = player.PersonId;
                            playerBoxScore.FullName = player.FullName;

                            vTeamPlayerStats.Add(playerBoxScore);
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

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
        private Game gameBoxScore = new Game();
        private List<Player> playersList = new List<Player>();
        private List<Team> teamList = new List<Team>();
        public ObservableCollection<ActivePlayerBoxScore> HTeamPlayerStats { get; set; }
        public ObservableCollection<ActivePlayerBoxScore> VTeamPlayerStats { get; set; }
        public ObservableCollection<ActivePlayerBoxScore> ListShowed { get; set; }
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
            parameters.Add("teams", teamList);
            parameters.Add("personId", playerSelected.PersonId);
            parameters.Add("players", playersList);

            await NavigationService.NavigateAsync(Config.PlayerProfilePage, parameters);
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
            var boxScore = await NbaApiService.GetBoxScore(gameBoxScore.StartDateEastern, gameBoxScore.GameId);

            if (boxScore.GetType().Name == "BoxScore")
            {
                if (boxScore != null)
                {

                    HTeamName = boxScore.BasicGameData.HTeam.TriCode;
                    VTeamName = boxScore.BasicGameData.VTeamBoxScore.TriCode;

                    GetTime(boxScore);

                    string hTeamId = boxScore.BasicGameData.HTeam.TeamId;
                    string vTeamId = boxScore.BasicGameData.VTeamBoxScore.TeamId;

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

                        foreach (Player item in playersList)
                        {


                            if (item.TeamId == hTeamId)
                            {
                                ActivePlayerBoxScore player = new ActivePlayerBoxScore();

                                player.FullName = $"{item.FirstName} {item.LastName}";
                                player.PersonId = item.PersonId;
                                hTeamPlayerStats.Add(player);
                            }
                            else if (item.TeamId == vTeamId)
                            {
                                ActivePlayerBoxScore player = new ActivePlayerBoxScore();

                                player.FullName = $"{item.FirstName} {item.LastName}";
                                player.PersonId = item.PersonId;
                                vTeamPlayerStats.Add(player);
                            }

                        }

                        HTeamPlayerStats = hTeamPlayerStats;
                        VTeamPlayerStats = vTeamPlayerStats;
                    }

                    ShowVTeam = true;

                }
            }
        }

        public void GetTime(BoxScore boxScore)
        {
            if (!string.IsNullOrEmpty(boxScore.BasicGameData.HTeam.Score) && $"{boxScore.BasicGameData.VTeamBoxScore.Score} - {boxScore.BasicGameData.HTeam.Score}" != "0 - 0")
            {

                ScoreOrTime = $"{boxScore.BasicGameData.VTeamBoxScore.Score} - {boxScore.BasicGameData.HTeam.Score}";

                if (boxScore.BasicGameData.Period.IsHalftime)
                {
                    TimePeriodHalftime = "HALFTIME";
                }
                else if (boxScore.BasicGameData.Period.IsEndOfPeriod && boxScore.BasicGameData.Period.Current <= 4)
                {
                    TimePeriodHalftime = $"END OF {boxScore.BasicGameData.Period.Current} PERIOD";
                }
                else if (boxScore.BasicGameData.Period.IsEndOfPeriod && boxScore.BasicGameData.Period.Current > 4)
                {
                    TimePeriodHalftime = $"END OF {boxScore.BasicGameData.Period.Current - 4} OT";
                }
                else if (!boxScore.BasicGameData.IsGameActivated)
                {
                    TimePeriodHalftime = "FINAL";
                }
                else if (boxScore.BasicGameData.Period.Current <= 4)
                {
                    TimePeriodHalftime = $"{boxScore.BasicGameData.Period.Current} PERIOD - {boxScore.BasicGameData.Clock} LEFT";
                }
                else if (boxScore.BasicGameData.Period.Current > 4)
                {
                    TimePeriodHalftime = $"{boxScore.BasicGameData.Period.Current - 4} OT - {boxScore.BasicGameData.Clock} LEFT";
                }
            }
            else
            {
                ScoreOrTime = boxScore.BasicGameData.StartTimeEastern;
            }
        }

        public void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("game", out Game game))
            {
                gameBoxScore = game;
            }

            if (parameters.TryGetValue("playersList", out List<Player> playersListParam))
            {
                playersList = playersListParam;
            }

            if (parameters.TryGetValue("teams", out List<Team> teams))
            {
                teamList = teams;
            }

            GetBoxScore();
        }
    }

}

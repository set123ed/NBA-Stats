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
    class CalendarViewModel : BaseViewModel
    {
        public ObservableCollection<Game> GamesOfTheDate { get; set; } = new ObservableCollection<Game>();
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
        public CalendarViewModel(INbaApiService nbaApiService, INavigationService navigationService, INbaDefaultInfoService nbaDefaultInfoService, IDataBaseServices baseServices) : base(navigationService, nbaApiService, nbaDefaultInfoService, baseServices)
        {
            DateSelectedChangeCommand = new Command(OnDateSelectedChange);
            RefreshGamesCommand = new Command(OnRefreshGames);

            OneDayLessCommand = new Command(OneDayLess);
            OneDayMoreCommand = new Command(OneDayMore);

            GameSelectedCommand = new Command<Game>(OnGameSelected);

            GetCalendarData();
        }

        private async void GetCalendarData()
        {
            await GetDefaultData();
            await GetGamesOfTheDate();
            IsBusy = false;
        }

        private async void OnGameSelected(Game game)
        {
            var parameters = new NavigationParameters();
            parameters.Add(ParametersConstants.GameId, game.GameId);
            parameters.Add(ParametersConstants.DateGame, game.StartDateEastern);

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
            try
            {
                GameOfDay gameOfDay = await NbaApiService.GetGamesOfDay(dateFormatted);

                if (gameOfDay != null)
                {
                    foreach (Game game in gameOfDay.Games)
                    {
                        game.ScoreOrTime = Utilities.GetScoreOrTime(game.VTeam.Score, game.HTeam.Score, game.StartTimeEastern);
                        game.TimePeriodHalftime = Utilities.GetTimePeriod(game.VTeam.Score, game.HTeam.Score, game.Period.CurrentPeriod, game.Period.IsHalftime, game.Period.IsEndOfPeriod, game.IsGameActivated, game.Clock);
                    }

                    GamesOfTheDate = new ObservableCollection<Game>(gameOfDay.Games);

                }

            }
            catch (NoInternetConnectionException ex)
            {

            }        

        }

    }
}

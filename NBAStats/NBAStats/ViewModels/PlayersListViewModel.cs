using NBAStats.Models;
using NBAStats.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NBAStats.ViewModels
{
    public class PlayersListViewModel: BaseViewModel
    {

        public ObservableCollection<Player> Players { get; set; }
        public ICommand DateSelectedChangeCommand { get; }
        public ICommand OneDayLessCommand { get; }
        public ICommand OneDayMoreCommand { get; }
        public DateTime SelectedTime { get; set; } = DateTime.Today;
        private string dateFormatted => SelectedTime.ToString("yyyy");

        public PlayersListViewModel(INbaApiService nbaApiService, INavigationService navigationService, INbaDefaultInfoService nbaDefaultInfoService) : base(navigationService, nbaApiService, nbaDefaultInfoService)
        {
            OneDayLessCommand = new Command(OneDayMore);
            OneDayMoreCommand = new Command(OneDayLess);
            // DateSelectedChangeCommand = new Command(async () => await GetNamePlayers());
            getdata();
        }
        public async void getdata()
        {
            await GetNamePlayers();
        }
        public void OneDayMore()
        {
            SelectedTime = SelectedTime.AddDays(+1);
        }
        public void OneDayLess()
        {
            SelectedTime = SelectedTime.AddDays(-1);
        }
        public async Task GetNamePlayers()
        {
            Players dateInfo = await NbaApiService.GetNbaPlayers(dateFormatted);
            if(dateInfo.League.Standard != null)
            {
                Players = new ObservableCollection<Player>(dateInfo.League.Standard);
            }
            else
            {
                Players = null;
            }
        }

    }
}

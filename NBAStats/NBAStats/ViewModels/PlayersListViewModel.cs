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

        public PlayersListViewModel(INbaApiService nbaApiServices, INavigationService navigationService) : base(navigationService, nbaApiServices)
        {
            OneDayLessCommand = new Command(OneDayMore);
            OneDayMoreCommand = new Command(OneDayLess);
            DateSelectedChangeCommand = new Command(async() => await GetNamePlayers());
            
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
            var dateInfo = await NbaApiService.GetNbaPlayers(dateFormatted);
            var NamePalyer = dateInfo.League.Standard;
            foreach(var datos in NamePalyer)
            {
                Players.Add(datos);
              
            }

            
        }

    }
}

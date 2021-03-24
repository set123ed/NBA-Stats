using NBAStats.Models.PlayersModel;
using NBAStats.Models.TeamsModels;
using NBAStats.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace NBAStats.ViewModels
{
    public class PlayersViewModel : BaseViewModel
    {
        private ObservableCollection<Standard> TeamList { get; set; }
        public ObservableCollection<Player> ActivePlayers { get; set; }
        public ObservableCollection<Player> AllActivePlayers { get; set; }
        public string PlayersFilter { get; set; }
        public bool InternetConnection { get; set; }
        public bool ShowError => !InternetConnection;
        public ICommand SelectedPlayerCommand { get; }
        public ICommand SearcherCommand { get; }
        public ICommand ClearCommand { get; }


        public PlayersViewModel(INbaApiService nbaApiServices) : base(nbaApiServices)
        {
            GetActivePlayers();
            GetActiveTeams();

            SelectedPlayerCommand = new Command<Player>(OnSelectecPlayer);
            SearcherCommand = new Command(OnSearcher);
            ClearCommand = new Command(OnClear);
        }

        private async void GetActiveTeams()
        {
            var teams = await NbaApiService.GetTeamsInformation();

            if (teams.GetType().Name == "TeamsList")
            {
                if (teams != null)
                {
                    TeamList = new ObservableCollection<Standard>(teams.League.Standard);
                    InternetConnection = true;
                }
            }
            else
            {
                InternetConnection = false;
            }
        }

        private async void OnSelectecPlayer(Player player)
        {
            Standard team = TeamList.First(t => t.TeamId == player.TeamId);
            //await NavigationServices.NonModalPush(new PlayerInfoDetailPage(player, team));
        }

        private void OnClear()
        {
            if (!string.IsNullOrEmpty(PlayersFilter))
            {
                if (ActivePlayers != AllActivePlayers)
                {
                    ActivePlayers = AllActivePlayers;
                }
                PlayersFilter = "";

            }
        }

        private void OnSearcher()
        {
            if (!string.IsNullOrEmpty(PlayersFilter))
            {
                List<Player> filteredPlayers = AllActivePlayers.Where(player => (player.FirstName.ToLower() + " " + player.LastName.ToLower()).Contains(PlayersFilter.ToLower())).ToList();
                ActivePlayers = new ObservableCollection<Player>(filteredPlayers);
            }
            else
            {
                if (ActivePlayers != AllActivePlayers)
                {
                    ActivePlayers = AllActivePlayers;
                }
            }
        }

        private async void GetActivePlayers()
        {
            var players = await NbaApiService.GetNbaPlayers();
            if (players.GetType().Name == "Players")
            {
                if (players != null)
                {
                    List<Player> activlePlayersList = players.League.Standard.Where(player => player.IsActive == true).ToList();
                    ActivePlayers = new ObservableCollection<Player>(activlePlayersList);
                    AllActivePlayers = new ObservableCollection<Player>(activlePlayersList);
                    InternetConnection = true;
                }
            }
            else
            {
                InternetConnection = false;
            }

        }
    }
}

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
    public class FavoritesViewModel : BaseViewModel
    {
        public ObservableCollection<FavoritesPlayer> FavoritesPlayers {get;set;}
        public ObservableCollection<FavoritesTeam> FavoritesTeams {get;set;}

        private ObservableCollection<FavoritesPlayer> _fullFavoritesPlayers = new ObservableCollection<FavoritesPlayer>();
        private ObservableCollection<FavoritesTeam> _fullFavoritesTeams = new ObservableCollection<FavoritesTeam>();


        public ObservableCollection<Player> NonFavoritePlayers { get; set; }
        public ObservableCollection<Team> NonFavoritesTeams { get; set; }

        public bool ShowFavoritesTeams { get; set; } = true;
        public bool ShowFavoritesPlayers => !ShowFavoritesTeams;

        public Player CurrentNonFavoritePlayerSelected { get; set; }
        public Team CurrentNonFavoriteTeamSelected { get; set; }
        public FavoritesPlayer CurrentFavoritePlayerSelected { get; set; }
        public FavoritesTeam CurrentFavoriteTeamSelected { get; set; }

        public string Filter { get; set; }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ShowFavoritesPlayersCommand { get; }
        public ICommand ShowFavoritesTeamsCommand { get; }

        public ICommand SearcherCommand { get; set; }
        public ICommand ClearCommand { get; set; }


        public FavoritesViewModel(INavigationService navigationService, INbaApiService nbaApiService, INbaDefaultInfoService nbaDefaultInfoService, IDatabaseService databaseService) : base(navigationService, nbaApiService, nbaDefaultInfoService, databaseService)
        {
            AddCommand = new Command(OnAdd);
            DeleteCommand = new Command(OnDelete);

            SearcherCommand = new Command(OnSearch);
            ClearCommand = new Command(OnClear);

            ShowFavoritesTeamsCommand = new Command(OnShowFavoritesTeams);
            ShowFavoritesPlayersCommand = new Command(OnShowFavoritesPlayers);

            GetFavoritesData();
        }

        private void OnClear()
        {
            if (!string.IsNullOrEmpty(Filter))
            {
                Filter = "";
            }
        }

        private void OnSearch()
        {
            if (!string.IsNullOrEmpty(Filter))
            {
                if (ShowFavoritesTeams)
                {
                    FavoritesTeams = new ObservableCollection<FavoritesTeam>(_fullFavoritesTeams.Where(team => team.Name.ToLower().Contains(Filter.ToLower())));
                    NonFavoritesTeams = new ObservableCollection<Team>(_teamList.Where(team => team.FullName.ToLower().Contains(Filter.ToLower())));
                }
                else
                {
                    FavoritesPlayers = new ObservableCollection<FavoritesPlayer>(_fullFavoritesPlayers.Where(player => player.Name.ToLower().Contains(Filter.ToLower())));
                    NonFavoritePlayers = new ObservableCollection<Player>(_playerList.Where(player => player.FullName.ToLower().Contains(Filter.ToLower())));
                }
            }
            else
            {
                if (ShowFavoritesTeams)
                {
                    FavoritesTeams = new ObservableCollection<FavoritesTeam>(_fullFavoritesTeams);
                    NonFavoritesTeams = new ObservableCollection<Team>(_teamList);
                }
                else
                {
                    FavoritesPlayers = new ObservableCollection<FavoritesPlayer>(_fullFavoritesPlayers);
                    NonFavoritePlayers = new ObservableCollection<Player>(_playerList);
                }

            }
        }

        private void OnShowFavoritesTeams()
        {
            if (!ShowFavoritesTeams)
            {
                if (!string.IsNullOrEmpty(Filter))
                {
                    Filter = "";
                }
                ShowFavoritesTeams = true;

                //if (FavoritesPlayers.Count != _fullFavoritesPlayers.Count)
                //{
                //    FavoritesPlayers = new ObservableCollection<FavoritesPlayer>(_fullFavoritesPlayers);
                //}

                //if (NonFavoritePlayers.Count != _playerList.Count)
                //{
                //    NonFavoritePlayers = new ObservableCollection<Player>(_playerList);
                //}
            }
        }

        private void OnShowFavoritesPlayers()
        {
            if (ShowFavoritesTeams)
            {
                if (!string.IsNullOrEmpty(Filter))
                {
                    Filter = "";
                }
                ShowFavoritesTeams = false;

                //if (FavoritesTeams.Count != _fullFavoritesTeams.Count)
                //{
                //    FavoritesTeams = new ObservableCollection<FavoritesTeam>(_fullFavoritesTeams);
                //}

                //if (NonFavoritesTeams.Count != _teamList.Count)
                //{
                //    NonFavoritesTeams = new ObservableCollection<Team>(_teamList);
                //}
            }
        }

        private async void OnAdd()
        {
            if (ShowFavoritesTeams)
            {
                await AddFavoriteTeam();
            }
            else
            {
                await AddFavoritePlayer();
            }
        }

        private async void OnDelete()
        {
            if (ShowFavoritesTeams)
            {
                await DeleteFavoriteTeam();
            }
            else
            {
                await DeleteFavoritePlayer();
            }
        }

        public async void GetFavoritesData()
        {
            await GetDefaultData();

            FavoritesPlayers = new ObservableCollection<FavoritesPlayer>(await DatabaseService.GetFavoritePlayers());
            _fullFavoritesPlayers = new ObservableCollection<FavoritesPlayer>(FavoritesPlayers);

            FavoritesTeams = new ObservableCollection<FavoritesTeam>(await DatabaseService.GetFavoriteTeams());
            _fullFavoritesTeams = new ObservableCollection<FavoritesTeam>(FavoritesTeams);

            NonFavoritePlayers = new ObservableCollection<Player>(_playerList);
            NonFavoritesTeams = new ObservableCollection<Team>(_teamList);
        }

        private async Task AddFavoritePlayer()
        {
            if (CurrentNonFavoritePlayerSelected != null)
            {
                Player player = CurrentNonFavoritePlayerSelected;

                FavoritesPlayer newFavoritePlayer = new FavoritesPlayer
                {
                    PlayerId = player.PersonId,
                    Name = player.FullName,
                    TeamId = player.TeamId,
                    TeamLogo = player.TeamLogo
                };

                if (!await DatabaseService.FavoritePlayerExists(newFavoritePlayer))
                {
                    await DatabaseService.SavePlayer(newFavoritePlayer);
                    NonFavoritePlayers.Remove(player);
                    FavoritesPlayers = new ObservableCollection<FavoritesPlayer>(await DatabaseService.GetFavoritePlayers());
                    _fullFavoritesPlayers = new ObservableCollection<FavoritesPlayer>(FavoritesPlayers);
                    CurrentNonFavoritePlayerSelected = null;
                }
                else
                {

                }
            }
        }

        private async Task AddFavoriteTeam()
        {
            if (CurrentNonFavoriteTeamSelected != null)
            {

                Team team = CurrentNonFavoriteTeamSelected;

                FavoritesTeam newFavoriteTeam = new FavoritesTeam
                {
                    Name = team.FullName,
                    TeamId = team.TeamId,
                    Tricode = team.Tricode,
                    TeamLogo = team.TeamLogo
                };

                if (!await DatabaseService.FavoriteTeamExists(newFavoriteTeam))
                {
                    await DatabaseService.SaveTeam(newFavoriteTeam);
                    NonFavoritesTeams.Remove(team);
                    FavoritesTeams = new ObservableCollection<FavoritesTeam>(await DatabaseService.GetFavoriteTeams());
                    _fullFavoritesTeams = new ObservableCollection<FavoritesTeam>(FavoritesTeams);
                    CurrentNonFavoriteTeamSelected = null;
                }
                else
                {

                }
            }
        }

        private async Task DeleteFavoritePlayer()
        {
            if (CurrentFavoritePlayerSelected != null)
            {
                await DatabaseService.DeleteFavoritePlayer(CurrentFavoritePlayerSelected);

                CurrentFavoritePlayerSelected = null;

                FavoritesPlayers = new ObservableCollection<FavoritesPlayer>(await DatabaseService.GetFavoritePlayers());
                _fullFavoritesPlayers = new ObservableCollection<FavoritesPlayer>(FavoritesPlayers);

            }

        }
        private async Task DeleteFavoriteTeam()
        {
            if (CurrentFavoriteTeamSelected != null)
            {
                await DatabaseService.DeleteFavoriteTeams(CurrentFavoriteTeamSelected);

                CurrentFavoriteTeamSelected = null;

                FavoritesTeams = new ObservableCollection<FavoritesTeam>(await DatabaseService.GetFavoriteTeams());
                _fullFavoritesTeams = new ObservableCollection<FavoritesTeam>(FavoritesTeams);
            }

        }
    }
}

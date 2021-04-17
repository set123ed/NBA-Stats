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

        private ObservableCollection<FavoritesPlayer> FavoritesPlayersSorted => new ObservableCollection<FavoritesPlayer>(AllFavoritesPlayers.OrderBy(player => player.Name));


        public ObservableCollection<Player> NonFavoritePlayers { get; set; }
        public ObservableCollection<Team> NonFavoritesTeams { get; set; }

        private ObservableCollection<Player> _fullNonFavoritesPlayers = new ObservableCollection<Player>();
        private ObservableCollection<Team> _fullNonFavoritesTeams = new ObservableCollection<Team>();

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
                    FavoritesTeams = new ObservableCollection<FavoritesTeam>(AllFavoritesTeams.Where(team => team.Name.ToLower().Contains(Filter.ToLower())));
                    NonFavoritesTeams = new ObservableCollection<Team>(_fullNonFavoritesTeams.Where(team => team.FullName.ToLower().Contains(Filter.ToLower())));
                }
                else
                {
                    FavoritesPlayers = new ObservableCollection<FavoritesPlayer>(FavoritesPlayersSorted.Where(player => player.Name.ToLower().Contains(Filter.ToLower())));
                    NonFavoritePlayers = new ObservableCollection<Player>(_fullNonFavoritesPlayers.Where(player => player.FullName.ToLower().Contains(Filter.ToLower())));
                }
            }
            else
            {
                if (ShowFavoritesTeams)
                {
                    FavoritesTeams = new ObservableCollection<FavoritesTeam>(AllFavoritesTeams);
                    NonFavoritesTeams = new ObservableCollection<Team>(_fullNonFavoritesTeams);
                }
                else
                {
                    FavoritesPlayers = new ObservableCollection<FavoritesPlayer>(FavoritesPlayersSorted);
                    NonFavoritePlayers = new ObservableCollection<Player>(_fullNonFavoritesPlayers);
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

            FavoritesPlayers = new ObservableCollection<FavoritesPlayer>(FavoritesPlayersSorted);

            FavoritesTeams = new ObservableCollection<FavoritesTeam>(AllFavoritesTeams);

            var favoritesPlayersId = FavoritesPlayers.Select(player => player.PlayerId).ToList();
            _fullNonFavoritesPlayers = new ObservableCollection<Player>(_playerList.Where(player => !favoritesPlayersId.Contains(player.PersonId)).OrderBy(player => player.FullName));

            var favoritesTeamsId = FavoritesTeams.Select(team => team.TeamId).ToList();
            _fullNonFavoritesTeams = new ObservableCollection<Team>(_teamList.Where(team => !team.IsAllStar).Where(team => !favoritesTeamsId.Contains(team.TeamId)));

            NonFavoritePlayers = new ObservableCollection<Player>(_fullNonFavoritesPlayers);
            NonFavoritesTeams = new ObservableCollection<Team>(_fullNonFavoritesTeams);
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
                    _fullNonFavoritesPlayers.Remove(player);

                    if (!string.IsNullOrEmpty(Filter) && ShowFavoritesPlayers)
                    {
                        FavoritesPlayers = new ObservableCollection<FavoritesPlayer>(FavoritesPlayersSorted.Where(p => p.Name.ToLower().Contains(Filter.ToLower())));
                    }
                    else
                    {
                        FavoritesPlayers = new ObservableCollection<FavoritesPlayer>(FavoritesPlayersSorted);
                    }

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
                    _fullNonFavoritesTeams.Remove(team);

                    if (!string.IsNullOrEmpty(Filter) && ShowFavoritesTeams)
                    {
                        FavoritesTeams = new ObservableCollection<FavoritesTeam>(AllFavoritesTeams.Where(t => t.Name.ToLower().Contains(Filter.ToLower())));
                    }
                    else
                    {
                        FavoritesTeams = new ObservableCollection<FavoritesTeam>(AllFavoritesTeams);
                    }
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

                Player favoritePlayerDeleted = _playerList.First(player => player.PersonId == CurrentFavoritePlayerSelected.PlayerId);
                CurrentFavoritePlayerSelected = null;

                NonFavoritePlayers.Add(favoritePlayerDeleted);
                _fullNonFavoritesPlayers.Add(favoritePlayerDeleted);

                if (!string.IsNullOrEmpty(Filter) && ShowFavoritesPlayers)
                {
                    NonFavoritePlayers = new ObservableCollection<Player>(NonFavoritePlayers.OrderBy(player => player.FullName).Where(player => player.FullName.ToLower().Contains(Filter.ToLower())));
                    FavoritesPlayers = new ObservableCollection<FavoritesPlayer>(FavoritesPlayersSorted.Where(player => player.Name.ToLower().Contains(Filter.ToLower())));
                }
                else
                {
                    NonFavoritePlayers = new ObservableCollection<Player>(NonFavoritePlayers.OrderBy(player => player.FullName));
                    FavoritesPlayers = new ObservableCollection<FavoritesPlayer>(FavoritesPlayersSorted);
                }

                _fullNonFavoritesPlayers = new ObservableCollection<Player>(_fullNonFavoritesPlayers.OrderBy(player => player.FullName));


            }

        }
        private async Task DeleteFavoriteTeam()
        {
            if (CurrentFavoriteTeamSelected != null)
            {
                await DatabaseService.DeleteFavoriteTeams(CurrentFavoriteTeamSelected);

                Team favoriteTeamDeleted = _teamList.First(team => team.TeamId == CurrentFavoriteTeamSelected.TeamId);
                CurrentFavoriteTeamSelected = null;

                NonFavoritesTeams.Add(favoriteTeamDeleted);
                _fullNonFavoritesTeams.Add(favoriteTeamDeleted);

                if (!string.IsNullOrEmpty(Filter) && ShowFavoritesTeams)
                {
                    NonFavoritesTeams = new ObservableCollection<Team>(NonFavoritesTeams.OrderBy(team => team.FullName).Where(team => team.FullName.ToLower().Contains(Filter.ToLower())));
                    FavoritesTeams = new ObservableCollection<FavoritesTeam>(AllFavoritesTeams.Where(team => team.Name.ToLower().Contains(Filter.ToLower())));
                }
                else
                {
                    NonFavoritesTeams = new ObservableCollection<Team>(NonFavoritesTeams.OrderBy(team => team.FullName));
                    FavoritesTeams = new ObservableCollection<FavoritesTeam>(AllFavoritesTeams);
                }

                _fullNonFavoritesTeams = new ObservableCollection<Team>(_fullNonFavoritesTeams.OrderBy(team => team.FullName));

            }

        }
    }
}

using NBAStats.Models;
using NBAStats.Services;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace NBAStats.ViewModels
{
    public class FavoriteViewModel:BaseViewModel
    {

        public string Code { get; set; } 
        public string Name { get; set; } 
        public DelegateCommand SaveData { get; }
        public DelegateCommand DeleteData { get; }
        public ObservableCollection<FavoritePlayer> ListPlayer { get; set; } = new ObservableCollection<FavoritePlayer>();

        public FavoriteViewModel(INbaApiService nbaApiService, INavigationService navigationService, INbaDefaultInfoService nbaDefaultInfoService, IDatabaseService baseServices) : base(navigationService, nbaApiService, nbaDefaultInfoService, baseServices)
        {
            SaveData = new DelegateCommand(SaveFavoritePlayer);
        }
        private void SaveFavoritePlayer()
        {
            if(!String.IsNullOrEmpty(Code) && !String.IsNullOrEmpty(Name))
            {
                FavoritePlayer player = new FavoritePlayer
                {
                    IdFavoritePlayer =  Code,
                    Name = Name
                };
                //var ListPlayers = baseServices.SavePlayer(player);
                //ListPlayer = new  ObservableCollection<FavoritePlayer>((IEnumerable<FavoritePlayer>)ListPlayers);
            }
        }

    }
}

using NBAStats.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NBAStats.ViewModels
{
    class TeamViewModel : BaseViewModel
    {
        public  ObservableCollection<string> Team { get; set; }

        public TeamViewModel(INbaApiService nbaApiServices, INavigationService navigationService) : base(navigationService, nbaApiServices)
        {

        }

    }
}

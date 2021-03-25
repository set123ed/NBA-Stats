using NBAStats.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats.ViewModels
{
    class StatsViewModel : BaseViewModel
    {
        public StatsViewModel(INbaApiService nbaApiServices, INavigationService navigationService) : base(navigationService, nbaApiServices)
        {

        }
    }
}

using NBAStats.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats.ViewModels
{
    class CoachViewModel : BaseViewModel
    {
        public CoachViewModel(INbaApiService nbaApiServices, INavigationService navigationService) : base(navigationService, nbaApiServices)
        {

        }
    }
}

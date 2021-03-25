using NBAStats.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        public LoginViewModel(INbaApiService nbaApiServices, INavigationService navigationService) : base(navigationService, nbaApiServices)
        {

        }
    }
}

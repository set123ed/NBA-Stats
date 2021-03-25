using NBAStats.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats.ViewModels
{
    class AccountSettingsViewModel : BaseViewModel
    {
        public AccountSettingsViewModel(INbaApiService nbaApiServices, INavigationService navigationService) : base(navigationService, nbaApiServices)
        {

        }
    }
}

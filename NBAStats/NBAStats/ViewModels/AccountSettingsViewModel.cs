using NBAStats.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats.ViewModels
{
    class AccountSettingsViewModel : BaseViewModel
    {
        public AccountSettingsViewModel(INbaApiService nbaApiService) : base(nbaApiService)
        {

        }
    }
}

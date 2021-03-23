using NBAStats.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        public LoginViewModel(INbaApiService nbaApiService) : base(nbaApiService)
        {

        }
    }
}

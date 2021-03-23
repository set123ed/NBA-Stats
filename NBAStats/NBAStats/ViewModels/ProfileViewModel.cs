using NBAStats.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats.ViewModels
{
    class ProfileViewModel : BaseViewModel
    {
        public ProfileViewModel(INbaApiService nbaApiService) : base(nbaApiService)
        {

        }
    }
}

using NBAStats.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats.ViewModels
{
    class PlayerProfileViewModel : BaseViewModel
    {
        public PlayerProfileViewModel(INbaApiService nbaApiService) : base(nbaApiService)
        {

        }
    }
}

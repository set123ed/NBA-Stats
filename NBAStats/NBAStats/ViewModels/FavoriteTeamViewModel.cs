using NBAStats.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats.ViewModels
{
    class FavoriteTeamViewModel : BaseViewModel
    {
        public FavoriteTeamViewModel(INbaApiService nbaApiService) : base(nbaApiService)
        {

        }
    }
}

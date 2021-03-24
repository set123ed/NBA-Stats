using NBAStats.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats.ViewModels
{
    class TeamViewModel : BaseViewModel
    {
        public TeamViewModel(INbaApiService nbaApiService) : base(nbaApiService)
        {

        }
    }
}

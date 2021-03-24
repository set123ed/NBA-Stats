using NBAStats.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats.ViewModels
{
    class StatsViewModel : BaseViewModel
    {
        public StatsViewModel(INbaApiService nbaApiService) : base(nbaApiService)
        {

        }
    }
}

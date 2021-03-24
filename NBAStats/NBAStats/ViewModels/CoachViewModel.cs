using NBAStats.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats.ViewModels
{
    class CoachViewModel : BaseViewModel
    {
        public CoachViewModel(INbaApiService nbaApiService) : base(nbaApiService)
        {

        }
    }
}

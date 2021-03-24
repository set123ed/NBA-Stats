using NBAStats.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats.ViewModels
{
    class BoxScoreViewModel : BaseViewModel
    {
        public BoxScoreViewModel(INbaApiService nbaApiService) : base(nbaApiService)
        {

        }
    }
}

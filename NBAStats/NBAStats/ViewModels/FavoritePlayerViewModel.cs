using NBAStats.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats.ViewModels
{
    class FavoritePlayerViewModel : BaseViewModel
    {
        public FavoritePlayerViewModel(INbaApiService nbaApiService) : base(nbaApiService)
        {

        }
    }
}

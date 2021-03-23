using NBAStats.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats.ViewModels
{


    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel(INbaApiService nbaApiService): base(nbaApiService)
        {

        }
    }
}

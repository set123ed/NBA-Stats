using NBAStats.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats.ViewModels
{
    class CalendarViewModel : BaseViewModel
    {
        public CalendarViewModel(INbaApiService nbaApiService) : base(nbaApiService)
        {

        }
    }
}

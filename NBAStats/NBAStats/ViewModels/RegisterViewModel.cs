﻿using NBAStats.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats.ViewModels
{
    class RegisterViewModel : BaseViewModel
    {
        public RegisterViewModel(INbaApiService nbaApiService) : base(nbaApiService)
        {

        }
    }
}
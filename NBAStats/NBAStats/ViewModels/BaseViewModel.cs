using NBAStats.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NBAStats.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public INbaApiService NbaApiService { get; set; }
        protected BaseViewModel(INbaApiService nbaApiService)
        {
            NbaApiService = nbaApiService;
        }
    }
}

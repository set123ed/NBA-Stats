using NBAStats.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NBAStats.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected INbaApiService NbaApiService { get;}
        protected INavigationService NavigationServie { get; }
        protected BaseViewModel(INavigationService navigationService, INbaApiService nbaApiService)
        {
            NbaApiService = nbaApiService;
            NavigationServie = navigationService;
        }
    }
}

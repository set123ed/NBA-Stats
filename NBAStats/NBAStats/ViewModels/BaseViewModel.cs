using NBAStats.Models;
using NBAStats.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace NBAStats.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public string _seasonYearApiData = null;
        public string _seasonApiStats = null;

        public event PropertyChangedEventHandler PropertyChanged;

        protected INbaApiService NbaApiService { get;}
        protected INavigationService NavigationService { get; }
        protected BaseViewModel(INavigationService navigationService, INbaApiService nbaApiService)
        {
            NbaApiService = nbaApiService;
            NavigationService = navigationService;
        }

        public async Task GetSeasonYearParameters()
        {
            var seasonRange = await NbaApiService.GetSeasonRange();

            if (seasonRange.GetType().Name == nameof(SeasonRange))
            {
                if (seasonRange != null)
                {
                    DateTime seasonStartDate = DateTime.ParseExact(seasonRange.StartDateCurrentSeason, "yyyyMMdd", CultureInfo.InvariantCulture);
                    _seasonYearApiData = seasonStartDate.Year.ToString();
                    _seasonApiStats = $"{_seasonYearApiData}-{seasonStartDate.AddYears(1).ToString("yy")}";
                }
            }
        }
    }
}

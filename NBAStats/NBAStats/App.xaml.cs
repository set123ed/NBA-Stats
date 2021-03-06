using NBAStats.Models;
using NBAStats.Services;
using NBAStats.ViewModels;
using NBAStats.Views;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Unity;
using SQLite;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NBAStats
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer platformInitializer = null) : base(platformInitializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            await NavigationService.NavigateAsync($"{NavigationConstants.TabbedPage}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NBATabbedPage>(NavigationConstants.TabbedPage);

            containerRegistry.Register<INbaApiService, NbaApiService>();
            containerRegistry.RegisterInstance<INbaDefaultInfoService>(new NbaDefaultInfoService());
            containerRegistry.RegisterInstance<IDatabaseService>(new DatabaseService());

            containerRegistry.RegisterForNavigation<FavoritesPage, FavoritesViewModel>(NavigationConstants.FavoritesPage);
            containerRegistry.RegisterForNavigation<HomePage, HomeViewModel>(NavigationConstants.HomePage);
            containerRegistry.RegisterForNavigation<BoxScorePage, BoxScoreViewModel>(NavigationConstants.BoxScorePage);
            containerRegistry.RegisterForNavigation<PlayerProfilePage, PlayerProfileViewModel>(NavigationConstants.PlayerProfilePage);
            containerRegistry.RegisterForNavigation<StandingPage, StandingViewModel>(NavigationConstants.StandingPage);
            containerRegistry.RegisterForNavigation<StatsPage, StatsViewModel>(NavigationConstants.StatsPage);
            containerRegistry.RegisterForNavigation<CalendarPage, CalendarViewModel>(NavigationConstants.CalendarPage);
            containerRegistry.RegisterForNavigation<TeamProfilePage, TeamProfileViewModel>(NavigationConstants.TeamProfilePage);
            containerRegistry.RegisterForNavigation<NavigationPage>(NavigationConstants.NavigationPage);
        }

    }
}

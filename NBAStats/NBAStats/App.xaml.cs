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
using Xamarin.Essentials;

namespace NBAStats
{
    public partial class App : PrismApplication
    {
        private static DataBaseService DataBase;
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

            NbaDefaultInfoService nbaDefaultInfoService = new NbaDefaultInfoService();
            //DataBaseService DataBase = new DataBaseService();
            containerRegistry.RegisterInstance<INbaDefaultInfoService>(nbaDefaultInfoService);
            containerRegistry.RegisterInstance<IDataBaseServices>(DataBase);

            containerRegistry.RegisterForNavigation<HomePage, HomeViewModel>(NavigationConstants.HomePage);
            containerRegistry.RegisterForNavigation<BoxScorePage, BoxScoreViewModel>(NavigationConstants.BoxScorePage);
            containerRegistry.RegisterForNavigation<PlayerProfilePage, PlayerProfileViewModel>(NavigationConstants.PlayerProfilePage);
            containerRegistry.RegisterForNavigation<StandingPage, StandingViewModel>(NavigationConstants.StandingPage);
            containerRegistry.RegisterForNavigation<StatsPage, StatsViewModel>(NavigationConstants.StatsPage);
            containerRegistry.RegisterForNavigation<CalendarPage, CalendarViewModel>(NavigationConstants.CalendarPage);
            containerRegistry.RegisterForNavigation<TeamProfilePage, TeamProfileViewModel>(NavigationConstants.TeamProfilePage);
            containerRegistry.RegisterForNavigation<PlayersListPage,PlayersListViewModel>(NavigationConstants.PlayersList);
            containerRegistry.RegisterForNavigation<FavoritePage, FavoriteViewModel>();
            containerRegistry.RegisterForNavigation<NavigationPage>(NavigationConstants.NavigationPage);
        }
        public static DataBaseService SQliteDB
        {
            get
            {
                if (DataBase == null)
                {
                    DataBase = new DataBaseService(Path.Combine(FileSystem.AppDataDirectory, "Stats.db3"));
                }
                return DataBase;
            }
        }

    }
}

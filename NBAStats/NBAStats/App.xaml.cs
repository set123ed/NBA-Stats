using NBAStats.Services;
using NBAStats.ViewModels;
using NBAStats.Views;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NBAStats
{
    public partial class App : PrismApplication
    {
        public Config constants = new Config();
        public App(IPlatformInitializer platformInitializer = null) : base(platformInitializer) { }
        
        protected override async void OnInitialized()
        {
            //var api = new NbaApiService();
            //var obj = await api.GetTeamStats();

            InitializeComponent();
            await NavigationService.NavigateAsync($"{Config.NavigationPage}/{Config.CalendarPage}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NBATabbedPage>(Config.TabbedPage);
            containerRegistry.Register<INbaApiService, NbaApiService>();

            containerRegistry.RegisterForNavigation<HomePage, HomeViewModel>(Config.HomePage);
            containerRegistry.RegisterForNavigation<BoxScorePage, BoxScoreViewModel>(Config.BoxScorePage);
            containerRegistry.RegisterForNavigation<PlayerProfilePage, PlayerProfileViewModel>(Config.PlayerProfilePage);
            containerRegistry.RegisterForNavigation<StandingPage, StandingViewModel>(Config.StandingPage);
            containerRegistry.RegisterForNavigation<StatsPage, StatsViewModel>(Config.StatsPage);
            containerRegistry.RegisterForNavigation<CalendarPage, CalendarViewModel>(Config.CalendarPage);

            containerRegistry.RegisterForNavigation<NavigationPage>(Config.NavigationPage);
            containerRegistry.RegisterForNavigation<PlayersPage, PlayersViewModel>(Config.PlayersPage);
            containerRegistry.RegisterForNavigation<PlayerInfoDetailPage, PlayerInfoDetailViewModel>(Config.PlayerInfoDetailPage);
            containerRegistry.RegisterForNavigation<TeamPage, TeamViewModel>(Config.TeamPage);
            containerRegistry.RegisterForNavigation<CoachPage, CoachViewModel>(Config.CoachPage);
        }
    }
}

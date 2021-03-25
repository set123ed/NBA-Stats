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
        public Config Constants = new Config();
        public App(IPlatformInitializer platformInitializer = null) : base(platformInitializer) { }
        
        protected override async void OnInitialized()
        {
            InitializeComponent();
            await NavigationService.NavigateAsync($"{Config.NavigationPage}/{Config.PlayersPage}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>(Config.NavigationPage);
            //containerRegistry.RegisterForNavigation<TeamPage, TeamViewModel>();
            containerRegistry.RegisterForNavigation<PlayersPage, PlayersViewModel>(Config.PlayersPage);
            containerRegistry.RegisterForNavigation<PlayerInfoDetailPage, PlayerInfoDetailViewModel>(Config.PlayerInfoDetailPage);
            containerRegistry.RegisterForNavigation<CoachPage, CoachViewModel>(Config.CoachPage);
        }
        //protected async override void OnStart()
        //{
        //    var coachService = new NbaApiService();
        //    var coachInfo = await coachService.GetNbaPlayers();

        //}
    }
}

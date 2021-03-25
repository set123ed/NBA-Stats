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
            await NavigationService.NavigateAsync($"{Config.NavigationPage}/{Config.TabbedPage}/{Config.PlayersPage}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NBATabbedPagePage>(Config.TabbedPage);
            containerRegistry.Register<INbaApiService, NbaApiService>();
            containerRegistry.RegisterForNavigation<NavigationPage>(Config.NavigationPage);
            containerRegistry.RegisterForNavigation<PlayersPage, PlayersViewModel>(Config.PlayersPage);
            containerRegistry.RegisterForNavigation<PlayerInfoDetailPage, PlayerInfoDetailViewModel>(Config.PlayerInfoDetailPage);
            containerRegistry.RegisterForNavigation<TeamPage, TeamViewModel>(Config.TeamPage);
        }
    }
}

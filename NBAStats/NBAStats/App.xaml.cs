using NBAStats.Constants;
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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NBAStats
{
    public partial class App : PrismApplication
    {
        static DataBaseService DataBase;
        public App(IPlatformInitializer platformInitializer = null) : base(platformInitializer) { }

        //List<Player> playerList = new List<Player>();
        //List<Team> teamList = new List<Team>();
        protected override async void OnInitialized()
        {
            InitializeComponent();
            await NavigationService.NavigateAsync($"{NavigationConstants.TabbedPage}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NBATabbedPage>(NavigationConstants.TabbedPage);
            containerRegistry.Register<INbaApiService, NbaApiService>();

            containerRegistry.RegisterForNavigation<HomePage, HomeViewModel>(NavigationConstants.HomePage);
            containerRegistry.RegisterForNavigation<BoxScorePage, BoxScoreViewModel>(NavigationConstants.BoxScorePage);
            containerRegistry.RegisterForNavigation<PlayerProfilePage, PlayerProfileViewModel>(NavigationConstants.PlayerProfilePage);
            containerRegistry.RegisterForNavigation<StandingPage, StandingViewModel>(NavigationConstants.StandingPage);
            containerRegistry.RegisterForNavigation<StatsPage, StatsViewModel>(NavigationConstants.StatsPage);
            containerRegistry.RegisterForNavigation<CalendarPage, CalendarViewModel>(NavigationConstants.CalendarPage);
            containerRegistry.RegisterForNavigation<TeamProfilePage, TeamProfileViewModel>(NavigationConstants.TeamProfilePage);

            containerRegistry.RegisterForNavigation<NavigationPage>(NavigationConstants.NavigationPage);
            //containerRegistry.RegisterForNavigation<PlayersPage, PlayersViewModel>(NavigationConstants.PlayersPage);
            //containerRegistry.RegisterForNavigation<PlayerInfoDetailPage, PlayerInfoDetailViewModel>(NavigationConstants.PlayerInfoDetailPage);
            //containerRegistry.RegisterForNavigation<TeamPage, TeamViewModel>(NavigationConstants.TeamPage);
            //containerRegistry.RegisterForNavigation<CoachPage, CoachViewModel>(NavigationConstants.CoachPage);
        }
        public static DataBaseService SQliteDB
        {
            get
            {
                if (DataBase == null)
                {
                    DataBase = new DataBaseService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Stats.db3"));
                }
                return DataBase;
            }
        }



        //private async Task GetPlayers()
        //{
        //    var nbaApiService = new NbaApiService();
        //    var players = await nbaApiService.GetNbaPlayers();

        //    if (players.GetType().Name == "Players")
        //    {
        //        if (players != null)
        //        {
        //            playerList = new List<Player>(players.League.Standard);
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception();
        //    }
        //}

        //public async Task GetTeams()
        //{
        //    var nbaApiService = new NbaApiService();
        //    var teams = await nbaApiService.GetTeams();

        //    if (teams.GetType().Name == "Teams")
        //    {
        //        if (teams != null)
        //        {
        //            teamList = teams.League.Standard;
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception();
        //    }
        //}
    }
}

using NBAStats.Models.PlayersModel;
using NBAStats.Models.TeamsModels;
using NBAStats.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats.ViewModels
{
    public class PlayerInfoDetailViewModel : BaseViewModel, IInitialize
    {
        public Player Player { get; set; }
        public Standard Team { get; set; }
        public string PlayerFullName { get; set; }
        public string PlayerHeight { get; set; }
        public string ActualTeamInfo { get; set; }
        public string YearDebutActualTeam { get; set; }
        public PlayerInfoDetailViewModel(INbaApiService nbaApiServices, INavigationService navigationService) : base(navigationService, nbaApiServices)
        {

        }

        public void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("team", out Standard team))
            {
                Team = team;
            }

            if (parameters.TryGetValue("player", out Player player))
            {
                Player = player;
            }

            PlayerFullName = Player.FirstName + " " + Player.LastName;
            PlayerHeight = $"{Player.HeightFeet}.{Player.HeightInches}";

            ActualTeamInfo = $"In {Team.Tricode} since: ";
            YearDebutActualTeam = Player.Teams[Player.Teams.Count - 1].SeasonStart;
        }


    }
}

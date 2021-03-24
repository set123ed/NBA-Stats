using NBAStats.Models.PlayersModel;
using NBAStats.Models.TeamsModels;
using NBAStats.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats.ViewModels
{
    public class PlayerInfoDetailViewModel : BaseViewModel
    {
        public Player Player { get; }
        public Standard Team { get; }
        public string PlayerFullName { get; }
        public string PlayerHeight { get; }
        public string ActualTeamInfo { get; }
        public string YearDebutActualTeam { get; }
        public PlayerInfoDetailViewModel(Player player, Standard team,  INbaApiService nbaApiService) : base(nbaApiService)
        {
            Player = player;
            Team = team;

            PlayerFullName = player.FirstName + " " + player.LastName;
            PlayerHeight = $"{player.HeightFeet}.{player.HeightInches}";

            ActualTeamInfo = $"In {team.Tricode} since: ";
            YearDebutActualTeam = player.Teams[player.Teams.Count - 1].SeasonStart;
        }


    }
}

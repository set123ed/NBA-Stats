using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace NBAStats.Models
{
    public class TeamSitesOnlyPlayers
    {

        [JsonPropertyName("playerCode")]
        public string PlayerCode { get; set; }

        [JsonPropertyName("posFull")]
        public string PosFull { get; set; }

        [JsonPropertyName("displayAffiliation")]
        public string DisplayAffiliation { get; set; }

        [JsonPropertyName("freeAgentCode")]
        public string FreeAgentCode { get; set; }
    }

    public class PlayerTeam
    {

        [JsonPropertyName("teamId")]
        public string TeamId { get; set; }

        [JsonPropertyName("seasonStart")]
        public string SeasonStart { get; set; }

        [JsonPropertyName("seasonEnd")]
        public string SeasonEnd { get; set; }
    }

    public class Draft
    {

        [JsonPropertyName("teamId")]
        public string TeamId { get; set; }

        [JsonPropertyName("pickNum")]
        public string PickNum { get; set; }

        [JsonPropertyName("roundNum")]
        public string RoundNum { get; set; }

        [JsonPropertyName("seasonYear")]
        public string SeasonYear { get; set; }
    }

    public class Player
    {
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("personId")]
        public string PersonId { get; set; }

        [JsonPropertyName("teamId")]
        public string TeamId { get; set; }

        [JsonPropertyName("jersey")]
        public string Jersey { get; set; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }

        [JsonPropertyName("pos")]
        public string Pos { get; set; }

        [JsonPropertyName("heightFeet")]
        public string HeightFeet { get; set; }

        [JsonPropertyName("heightInches")]
        public string HeightInches { get; set; }

        [JsonPropertyName("weightPounds")]
        public string WeightPounds { get; set; }

        [JsonPropertyName("dateOfBirthUTC")]
        public string DateOfBirthUTC { get; set; }

        [JsonPropertyName("teamSitesOnly")]
        public TeamSitesOnlyPlayers TeamSitesOnly { get; set; }

        [JsonPropertyName("teams")]
        public IList<PlayerTeam> Teams { get; set; }

        [JsonPropertyName("draft")]
        public Draft Draft { get; set; }

        [JsonPropertyName("yearsPro")]
        public string YearsPro { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("isallStar")]
        public bool? IsallStar { get; set; }

        public string FullName => $"{FirstName} {LastName}";
        public string PlayerHeight => $"{HeightFeet}.{HeightInches}";
        public string YearDebutActualTeam => Teams[Teams.Count - 1].SeasonStart;
        public string TeamLogo => $"{StringConstants.Logo}{TeamId}.png";
        public bool IsFavorite { get; set; } = false;
    }

    public class LeaguePlayer
    {

        [JsonPropertyName("standard")]
        public IList<Player> Standard { get; set; }

    }

    public class Players : INotifyPropertyChanged
    {


        [JsonPropertyName("league")]
        public LeaguePlayer League { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

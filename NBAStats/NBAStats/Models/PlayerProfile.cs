using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json.Serialization;

namespace NBAStats.Models
{


    public class StatsPlayerProfile
    {
        [JsonPropertyName("teamId")]
        public string TeamId { get; set; }
        [JsonPropertyName("seasonYear")]
        public int SeasonYear { get; set; }
        public string TriCodeTeam { get; set; }
        [JsonPropertyName("ppg")]
        public string Ppg { get; set; }

        [JsonPropertyName("rpg")]
        public string Rpg { get; set; }

        [JsonPropertyName("apg")]
        public string Apg { get; set; }

        [JsonPropertyName("mpg")]
        public string Mpg { get; set; }

        [JsonPropertyName("topg")]
        public string Topg { get; set; }

        [JsonPropertyName("spg")]
        public string Spg { get; set; }

        [JsonPropertyName("bpg")]
        public string Bpg { get; set; }

        [JsonPropertyName("tpp")]
        public string Tpp { get; set; }

        [JsonPropertyName("ftp")]
        public string Ftp { get; set; }

        [JsonPropertyName("fgp")]
        public string Fgp { get; set; }

        [JsonPropertyName("assists")]
        public string Assists { get; set; }

        [JsonPropertyName("blocks")]
        public string Blocks { get; set; }

        [JsonPropertyName("steals")]
        public string Steals { get; set; }

        [JsonPropertyName("turnovers")]
        public string Turnovers { get; set; }

        [JsonPropertyName("offReb")]
        public string OffReb { get; set; }

        [JsonPropertyName("defReb")]
        public string DefReb { get; set; }

        [JsonPropertyName("totReb")]
        public string TotReb { get; set; }

        [JsonPropertyName("fgm")]
        public string Fgm { get; set; }

        [JsonPropertyName("fga")]
        public string Fga { get; set; }

        [JsonPropertyName("tpm")]
        public string Tpm { get; set; }

        [JsonPropertyName("tpa")]
        public string Tpa { get; set; }

        [JsonPropertyName("ftm")]
        public string Ftm { get; set; }

        [JsonPropertyName("fta")]
        public string Fta { get; set; }

        [JsonPropertyName("pFouls")]
        public string PFouls { get; set; }

        [JsonPropertyName("points")]
        public string Points { get; set; }

        [JsonPropertyName("gamesPlayed")]
        public string GamesPlayed { get; set; }

        [JsonPropertyName("gamesStarted")]
        public string GamesStarted { get; set; }

        [JsonPropertyName("plusMinus")]
        public string PlusMinus { get; set; }

        [JsonPropertyName("min")]
        public string Min { get; set; }

        [JsonPropertyName("dd2")]
        public string Dd2 { get; set; }

        [JsonPropertyName("td3")]
        public string Td3 { get; set; }
        public string TeamLogo => $"{StringConstants.Logo}{TeamId}.png";
    }

    public class SeasonPlayerProfile
    {

        [JsonPropertyName("seasonYear")]
        public int SeasonYear { get; set; }

        [JsonPropertyName("teams")]
        public ObservableCollection<StatsPlayerProfile> Teams { get; set; }

    }

    public class RegularSeasonPlayerProfile
    {

        [JsonPropertyName("season")]
        public ObservableCollection<SeasonPlayerProfile> Season { get; set; }
    }

    public class StatsPerSeasonCollection : ObservableCollection<StatsPlayerProfile>
    {
        public StatsPerSeasonCollection(string seasonYear)
        {
            SeasonYear = seasonYear;
        }

        public string SeasonYear { get;  }

    }

    public class Stats
    {

        [JsonPropertyName("latest")]
        public StatsPlayerProfile Latest { get; set; }

        [JsonPropertyName("careerSummary")]
        public StatsPlayerProfile CareerSummary { get; set; }

        [JsonPropertyName("regularSeason")]
        public RegularSeasonPlayerProfile RegularSeason { get; set; }
    }

    public class PlayerStandard
    {

        [JsonPropertyName("teamId")]
        public string TeamId { get; set; }

        [JsonPropertyName("stats")]
        public Stats Stats { get; set; }
    }

    public class LeaguePlayerProfile
    {

        [JsonPropertyName("standard")]
        public PlayerStandard Standard { get; set; }
    }

    public class PlayerProfile
    {

        [JsonPropertyName("league")]
        public LeaguePlayerProfile League { get; set; }
    }


}

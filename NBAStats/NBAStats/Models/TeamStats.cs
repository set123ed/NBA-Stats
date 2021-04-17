using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json.Serialization;

namespace NBAStats.Models
{


    public class StatValueTeamStats
    {

        [JsonPropertyName("avg")]
        public string Avg { get; set; }

        [JsonPropertyName("rank")]
        public string Rank { get; set; }
    }

    public class TeamStats
    {

        [JsonPropertyName("teamId")]
        public string TeamId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("nickname")]
        public string Nickname { get; set; }

        [JsonPropertyName("teamcode")]
        public string Teamcode { get; set; }

        [JsonPropertyName("abbreviation")]
        public string Abbreviation { get; set; }

        [JsonPropertyName("min")]
        public StatValueTeamStats Min { get; set; }

        [JsonPropertyName("fgp")]
        public StatValueTeamStats Fgp { get; set; }

        [JsonPropertyName("tpp")]
        public StatValueTeamStats Tpp { get; set; }

        [JsonPropertyName("ftp")]
        public StatValueTeamStats Ftp { get; set; }

        [JsonPropertyName("orpg")]
        public StatValueTeamStats Orpg { get; set; }

        [JsonPropertyName("drpg")]
        public StatValueTeamStats Drpg { get; set; }

        [JsonPropertyName("trpg")]
        public StatValueTeamStats Trpg { get; set; }

        [JsonPropertyName("apg")]
        public StatValueTeamStats Apg { get; set; }

        [JsonPropertyName("tpg")]
        public StatValueTeamStats Tpg { get; set; }

        [JsonPropertyName("spg")]
        public StatValueTeamStats Spg { get; set; }

        [JsonPropertyName("bpg")]
        public StatValueTeamStats Bpg { get; set; }

        [JsonPropertyName("pfpg")]
        public StatValueTeamStats Pfpg { get; set; }

        [JsonPropertyName("ppg")]
        public StatValueTeamStats Ppg { get; set; }

        [JsonPropertyName("oppg")]
        public StatValueTeamStats Oppg { get; set; }

        [JsonPropertyName("eff")]
        public StatValueTeamStats Eff { get; set; }
    }

    public class Preseason
    {

        [JsonPropertyName("teams")]
        public ObservableCollection<TeamStats> Teams { get; set; }
    }

    public class RegularSeason
    {

        [JsonPropertyName("teams")]
        public ObservableCollection<TeamStats> Teams { get; set; }
    }


    public class Series
    {

        [JsonPropertyName("seriesId")]
        public string SeriesId { get; set; }

        [JsonPropertyName("teams")]
        public ObservableCollection<TeamStats> Teams { get; set; }
    }

    public class Playoffs
    {

        [JsonPropertyName("series")]
        public ObservableCollection<Series> Series { get; set; }
    }

    public class SeasonType
    {

        [JsonPropertyName("seasonYear")]
        public int SeasonYear { get; set; }

        [JsonPropertyName("preseason")]
        public Preseason Preseason { get; set; }

        [JsonPropertyName("regularSeason")]
        public RegularSeason RegularSeason { get; set; }

        [JsonPropertyName("playoffs")]
        public Playoffs Playoffs { get; set; }
    }
    public class LeagueTeamStats
    {

        [JsonPropertyName("standard")]
        public SeasonType Seasons { get; set; }

    }

    public class TeamStatsClass
    {
        [JsonPropertyName("league")]
        public LeagueTeamStats LeagueTeamStats { get; set; }
    }

    public class LeadersStatsTeam
    {
        public string Pos { get; set; }
        public string TeamId { get; set; }
        public string FullName { get; set; }
        public string AverageStats { get; set; }
        public string TeamLogo => $"{StringConstants.Logo}{TeamId}.png";
        public bool IsFavorite { get; set; } = false;
    }

    public class LeaderStatsTeamCollection : ObservableCollection<LeadersStatsTeam>
    {
        public LeaderStatsTeamCollection(string stat)
        {
            Stat = stat;
        }

        public string Stat { get;  }
    }
}


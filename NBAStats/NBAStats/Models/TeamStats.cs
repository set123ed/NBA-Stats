using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json.Serialization;

namespace NBAStats.Models
{


    public class MinTeamStats
    {

        [JsonPropertyName("avg")]
        public string Avg { get; set; }

        [JsonPropertyName("rank")]
        public string Rank { get; set; }
    }

    public class FgpTeamStats
    {

        [JsonPropertyName("avg")]
        public string Avg { get; set; }

        [JsonPropertyName("rank")]
        public string Rank { get; set; }
    }

    public class TppTeamStats
    {

        [JsonPropertyName("avg")]
        public string Avg { get; set; }

        [JsonPropertyName("rank")]
        public string Rank { get; set; }
    }

    public class FtpTeamStats
    {

        [JsonPropertyName("avg")]
        public string Avg { get; set; }

        [JsonPropertyName("rank")]
        public string Rank { get; set; }
    }

    public class OrpgTeamStats
    {

        [JsonPropertyName("avg")]
        public string Avg { get; set; }

        [JsonPropertyName("rank")]
        public string Rank { get; set; }
    }

    public class DrpgTeamStats
    {

        [JsonPropertyName("avg")]
        public string Avg { get; set; }

        [JsonPropertyName("rank")]
        public string Rank { get; set; }
    }

    public class TrpgTeamStats
    {

        [JsonPropertyName("avg")]
        public string Avg { get; set; }

        [JsonPropertyName("rank")]
        public string Rank { get; set; }
    }

    public class ApgTeamStats
    {

        [JsonPropertyName("avg")]
        public string Avg { get; set; }

        [JsonPropertyName("rank")]
        public string Rank { get; set; }
    }

    public class TpgTeamStats
    {

        [JsonPropertyName("avg")]
        public string Avg { get; set; }

        [JsonPropertyName("rank")]
        public string Rank { get; set; }
    }

    public class SpgTeamStats
    {

        [JsonPropertyName("avg")]
        public string Avg { get; set; }

        [JsonPropertyName("rank")]
        public string Rank { get; set; }
    }

    public class BpgTeamStats
    {

        [JsonPropertyName("avg")]
        public string Avg { get; set; }

        [JsonPropertyName("rank")]
        public string Rank { get; set; }
    }

    public class PfpgTeamStats
    {

        [JsonPropertyName("avg")]
        public string Avg { get; set; }

        [JsonPropertyName("rank")]
        public string Rank { get; set; }
    }

    public class PpgTeamStats
    {

        [JsonPropertyName("avg")]
        public string Avg { get; set; }

        [JsonPropertyName("rank")]
        public string Rank { get; set; }
    }

    public class OppgTeamStats
    {

        [JsonPropertyName("avg")]
        public string Avg { get; set; }

        [JsonPropertyName("rank")]
        public string Rank { get; set; }
    }

    public class EffTeamStats
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
        public MinTeamStats Min { get; set; }

        [JsonPropertyName("fgp")]
        public FgpTeamStats Fgp { get; set; }

        [JsonPropertyName("tpp")]
        public TppTeamStats Tpp { get; set; }

        [JsonPropertyName("ftp")]
        public FtpTeamStats Ftp { get; set; }

        [JsonPropertyName("orpg")]
        public OrpgTeamStats Orpg { get; set; }

        [JsonPropertyName("drpg")]
        public DrpgTeamStats Drpg { get; set; }

        [JsonPropertyName("trpg")]
        public TrpgTeamStats Trpg { get; set; }

        [JsonPropertyName("apg")]
        public ApgTeamStats Apg { get; set; }

        [JsonPropertyName("tpg")]
        public TpgTeamStats Tpg { get; set; }

        [JsonPropertyName("spg")]
        public SpgTeamStats Spg { get; set; }

        [JsonPropertyName("bpg")]
        public BpgTeamStats Bpg { get; set; }

        [JsonPropertyName("pfpg")]
        public PfpgTeamStats Pfpg { get; set; }

        [JsonPropertyName("ppg")]
        public PpgTeamStats Ppg { get; set; }

        [JsonPropertyName("oppg")]
        public OppgTeamStats Oppg { get; set; }

        [JsonPropertyName("eff")]
        public EffTeamStats Eff { get; set; }
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
    }
}


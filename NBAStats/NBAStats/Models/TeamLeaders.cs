using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NBAStats.Models
{

    public class StatTeamLeaders
    {

        [JsonPropertyName("personId")]
        public string PersonId { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class NbaTeamLeaders
    {

        [JsonPropertyName("seasonStageId")]
        public int SeasonStageId { get; set; }

        [JsonPropertyName("ppg")]
        public IList<StatTeamLeaders> Ppg { get; set; }

        [JsonPropertyName("trpg")]
        public IList<StatTeamLeaders> Trpg { get; set; }

        [JsonPropertyName("apg")]
        public IList<StatTeamLeaders> Apg { get; set; }

        [JsonPropertyName("fgp")]
        public IList<StatTeamLeaders> Fgp { get; set; }

        [JsonPropertyName("tpp")]
        public IList<StatTeamLeaders> Tpp { get; set; }

        [JsonPropertyName("ftp")]
        public IList<StatTeamLeaders> Ftp { get; set; }

        [JsonPropertyName("bpg")]
        public IList<StatTeamLeaders> Bpg { get; set; }

        [JsonPropertyName("spg")]
        public IList<StatTeamLeaders> Spg { get; set; }

        [JsonPropertyName("tpg")]
        public IList<StatTeamLeaders> Tpg { get; set; }

        [JsonPropertyName("pfpg")]
        public IList<StatTeamLeaders> Pfpg { get; set; }
    }


    public class LeagueTeamLeaders
    {

        [JsonPropertyName("standard")]
        public NbaTeamLeaders NbaTeamLeaders { get; set; }
    }

    public class TeamLeaders
    {
        [JsonPropertyName("league")]
        public LeagueTeamLeaders LeagueTeamLeaders { get; set; }
    }

    public class TeamLeadersPlayers
    {
        public string PlayerId { get; set; }
        public string FullName { get; set; }
        public string StatName { get; set; }
        public string StatAvg { get; set; }
        public bool IsFavorite { get; set; } = false;
    }
}

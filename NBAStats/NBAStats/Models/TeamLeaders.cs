using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NBAStats.Models
{

    public class PpgTeamLeaders
    {

        [JsonPropertyName("personId")]
        public string PersonId { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class TrpgTeamLeaders
    {

        [JsonPropertyName("personId")]
        public string PersonId { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class ApgTeamLeaders
    {

        [JsonPropertyName("personId")]
        public string PersonId { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class FgpTeamLeaders
    {

        [JsonPropertyName("personId")]
        public string PersonId { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class TppTeamLeaders
    {

        [JsonPropertyName("personId")]
        public string PersonId { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class FtpTeamLeaders
    {

        [JsonPropertyName("personId")]
        public string PersonId { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class BpgTeamLeaders
    {

        [JsonPropertyName("personId")]
        public string PersonId { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class SpgTeamLeaders
    {

        [JsonPropertyName("personId")]
        public string PersonId { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class TpgTeamLeaders
    {

        [JsonPropertyName("personId")]
        public string PersonId { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class PfpgTeamLeaders
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
        public IList<PpgTeamLeaders> Ppg { get; set; }

        [JsonPropertyName("trpg")]
        public IList<TrpgTeamLeaders> Trpg { get; set; }

        [JsonPropertyName("apg")]
        public IList<ApgTeamLeaders> Apg { get; set; }

        [JsonPropertyName("fgp")]
        public IList<FgpTeamLeaders> Fgp { get; set; }

        [JsonPropertyName("tpp")]
        public IList<TppTeamLeaders> Tpp { get; set; }

        [JsonPropertyName("ftp")]
        public IList<FtpTeamLeaders> Ftp { get; set; }

        [JsonPropertyName("bpg")]
        public IList<BpgTeamLeaders> Bpg { get; set; }

        [JsonPropertyName("spg")]
        public IList<SpgTeamLeaders> Spg { get; set; }

        [JsonPropertyName("tpg")]
        public IList<TpgTeamLeaders> Tpg { get; set; }

        [JsonPropertyName("pfpg")]
        public IList<PfpgTeamLeaders> Pfpg { get; set; }
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
    }
}

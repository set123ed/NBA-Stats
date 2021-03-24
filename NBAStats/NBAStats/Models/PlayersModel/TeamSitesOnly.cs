using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NBAStats.Models.PlayersModel
{
    public class TeamSitesOnly
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
}

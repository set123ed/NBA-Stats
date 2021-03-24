using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NBAStats.Models.PlayersModel
{
    public class PlayerTeam
    {

        [JsonPropertyName("teamId")]
        public string TeamId { get; set; }

        [JsonPropertyName("seasonStart")]
        public string SeasonStart { get; set; }

        [JsonPropertyName("seasonEnd")]
        public string SeasonEnd { get; set; }
    }
}

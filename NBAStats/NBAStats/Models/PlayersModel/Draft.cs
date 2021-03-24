using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NBAStats.Models.PlayersModel
{
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
}

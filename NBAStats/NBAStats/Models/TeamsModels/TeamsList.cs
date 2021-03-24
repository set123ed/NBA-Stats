using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NBAStats.Models.TeamsModels
{
    public class TeamsList
    {
        [JsonPropertyName("_internal")]
        public TeamsInformation TeamsInformation { get; set; }

        [JsonPropertyName("league")]
        public League League { get; set; }
    }
}

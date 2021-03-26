using System;
using System.Text.Json.Serialization;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
namespace NBAStats.Models.CoachModels
{
    public class TeamSitesOnly
    {

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("coachCode")]
        public string CoachCode { get; set; }

        [JsonPropertyName("coachRole")]
        public string CoachRole { get; set; }

        [JsonPropertyName("teamCode")]
        public string TeamCode { get; set; }

        [JsonPropertyName("teamTricode")]
        public string TeamTricode { get; set; }
    }

}

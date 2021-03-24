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
        public string displayName { get; set; }

        [JsonPropertyName("coachCode")]
        public string coachCode { get; set; }

        [JsonPropertyName("coachRole")]
        public string coachRole { get; set; }

        [JsonPropertyName("teamCode")]
        public string teamCode { get; set; }

        [JsonPropertyName("teamTricode")]
        public string teamTricode { get; set; }
    }

}

using System;
using System.Text.Json.Serialization;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
namespace NBAStats.Models.CoachModels
{
   
        public class Standard
        {

            [JsonPropertyName("firstName")]
            public string firstName { get; set; }

            [JsonPropertyName("lastName")]
            public string lastName { get; set; }

            [JsonPropertyName("isAssistant")]
            public bool isAssistant { get; set; }

            [JsonPropertyName("personId")]
            public string personId { get; set; }

            [JsonPropertyName("teamId")]
            public string teamId { get; set; }

            [JsonPropertyName("sortSequence")]
            public string sortSequence { get; set; }

            [JsonPropertyName("college")]
            public string college { get; set; }

            [JsonPropertyName("teamSitesOnly")]
            public TeamSitesOnly teamSitesOnly { get; set; }
        }

    
}

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
            public string FirstName { get; set; }

            [JsonPropertyName("lastName")]
            public string LastName { get; set; }

            [JsonPropertyName("isAssistant")]
            public bool IsAssistant { get; set; }

            [JsonPropertyName("personId")]
            public string PersonId { get; set; }

            [JsonPropertyName("teamId")]
            public string TeamId { get; set; }

            [JsonPropertyName("sortSequence")]
            public string SortSequence { get; set; }

            [JsonPropertyName("college")]
            public string College { get; set; }

            [JsonPropertyName("teamSitesOnly")]
            public TeamSitesOnly TeamSitesOnly { get; set; }
        }

    
}

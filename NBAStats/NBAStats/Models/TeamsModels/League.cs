using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NBAStats.Models.TeamsModels
{
    public class League
    {
        [JsonPropertyName("standard")]
        public IList<Standard> Standard { get; set; }

        [JsonPropertyName("africa")]
        public IList<object> Africa { get; set; }

        [JsonPropertyName("sacramento")]
        public IList<object> Sacramento { get; set; }

        [JsonPropertyName("vegas")]
        public IList<object> Vegas { get; set; }

        [JsonPropertyName("utah")]
        public IList<object> Utah { get; set; }
    }
}

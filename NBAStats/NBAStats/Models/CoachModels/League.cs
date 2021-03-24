using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NBAStats.Models.CoachModels
{

    public class League
    {
        [JsonPropertyName("standard")]
        public IList<Standard> Standard { get; set; }
    }
}

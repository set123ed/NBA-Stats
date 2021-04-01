using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NBAStats.Models
{
    public class SeasonRange
    {

        [JsonPropertyName("startDate")]
        public string StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public string EndDate { get; set; }

        [JsonPropertyName("startDateCurrentSeason")]
        public string StartDateCurrentSeason { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NBAStats.Models.TeamsModels
{
    public class Standard
    {
        [JsonPropertyName("isNBAFranchise")]
        public bool IsNBAFranchise { get; set; }

        [JsonPropertyName("isAllStar")]
        public bool IsAllStar { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("altCityName")]
        public string AltCityName { get; set; }

        [JsonPropertyName("fullName")]
        public string FullName { get; set; }

        [JsonPropertyName("tricode")]
        public string Tricode { get; set; }

        [JsonPropertyName("teamId")]
        public string TeamId { get; set; }

        [JsonPropertyName("nickname")]
        public string Nickname { get; set; }

        [JsonPropertyName("urlName")]
        public string UrlName { get; set; }

        [JsonPropertyName("teamShortName")]
        public string TeamShortName { get; set; }

        [JsonPropertyName("confName")]
        public string ConfName { get; set; }

        [JsonPropertyName("divName")]
        public string DivName { get; set; }
    }
}

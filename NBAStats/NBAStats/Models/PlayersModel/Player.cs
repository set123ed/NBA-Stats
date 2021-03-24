using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json.Serialization;

namespace NBAStats.Models.PlayersModel
{
    public class Player
    {

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("temporaryDisplayName")]
        public string TemporaryDisplayName { get; set; }

        [JsonPropertyName("personId")]
        public string PersonId { get; set; }

        [JsonPropertyName("teamId")]
        public string TeamId { get; set; }

        [JsonPropertyName("jersey")]
        public string Jersey { get; set; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }

        [JsonPropertyName("pos")]
        public string Pos { get; set; }

        [JsonPropertyName("heightFeet")]
        public string HeightFeet { get; set; }

        [JsonPropertyName("heightInches")]
        public string HeightInches { get; set; }

        [JsonPropertyName("heightMeters")]
        public string HeightMeters { get; set; }

        [JsonPropertyName("weightPounds")]
        public string WeightPounds { get; set; }

        [JsonPropertyName("weightKilograms")]
        public string WeightKilograms { get; set; }

        [JsonPropertyName("dateOfBirthUTC")]
        public string DateOfBirthUTC { get; set; }

        [JsonPropertyName("teamSitesOnly")]
        public TeamSitesOnly TeamSitesOnly { get; set; }

        [JsonPropertyName("teams")]
        public ObservableCollection<PlayerTeam> Teams { get; set; }

        [JsonPropertyName("draft")]
        public Draft Draft { get; set; }

        [JsonPropertyName("nbaDebutYear")]
        public string NbaDebutYear { get; set; }

        [JsonPropertyName("yearsPro")]
        public string YearsPro { get; set; }

        [JsonPropertyName("collegeName")]
        public string CollegeName { get; set; }

        [JsonPropertyName("lastAffiliation")]
        public string LastAffiliation { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("isallStar")]
        public bool? IsallStar { get; set; }
    }
}

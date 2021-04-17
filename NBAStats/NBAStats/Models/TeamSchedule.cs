using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json.Serialization;

namespace NBAStats.Models
{
    public class TeamInfoSchedule
    {
        [JsonPropertyName("teamId")]
        public string TeamId { get; set; }
        [JsonPropertyName("score")]
        public string Score { get; set; }
        public string Tricode { get; set; }
        public string TeamLogo => $"{StringConstants.Logo}{TeamId}.png";
        public bool IsFavorite { get; set; } = false;
    }


    public class GameTeamSchedule
    {

        [JsonPropertyName("seasonStageId")]
        public int SeasonStageId { get; set; }

        [JsonPropertyName("seasonId")]
        public string SeasonId { get; set; }

        [JsonPropertyName("gameUrlCode")]
        public string GameUrlCode { get; set; }

        [JsonPropertyName("gameId")]
        public string GameId { get; set; }

        [JsonPropertyName("statusNum")]
        public int StatusNum { get; set; }

        [JsonPropertyName("extendedStatusNum")]
        public int ExtendedStatusNum { get; set; }

        [JsonPropertyName("startTimeEastern")]
        public string StartTimeEastern { get; set; }

        [JsonPropertyName("startTimeUTC")]
        public object StartTimeUTC { get; set; }

        [JsonPropertyName("startDateEastern")]
        public string StartDateEastern { get; set; }

        [JsonPropertyName("homeStartDate")]
        public string HomeStartDate { get; set; }

        [JsonPropertyName("homeStartTime")]
        public string HomeStartTime { get; set; }

        [JsonPropertyName("visitorStartDate")]
        public string VisitorStartDate { get; set; }

        [JsonPropertyName("visitorStartTime")]
        public string VisitorStartTime { get; set; }

        [JsonPropertyName("isHomeTeam")]
        public bool IsHomeTeam { get; set; }

        [JsonPropertyName("isStartTimeTBD")]
        public bool IsStartTimeTBD { get; set; }

        [JsonPropertyName("vTeam")]
        public TeamInfoSchedule VTeam { get; set; }

        [JsonPropertyName("hTeam")]
        public TeamInfoSchedule HTeam { get; set; }

        public string ScoreOrTime { get; set; }

        public string SeasonStage { get; set; }
        
        public string Result { get; set; }

    }

    public class GameScheduleCollection : ObservableCollection<GameTeamSchedule>
    {
        public GameScheduleCollection(string filter)
        {
            Filter = filter;
        }

        public string Filter { get; }

    }

    public class LeagueTeamSchedule
    {

        [JsonPropertyName("lastStandardGamePlayedIndex")]
        public int LastStandardGamePlayedIndex { get; set; }

        [JsonPropertyName("standard")]
        public ObservableCollection<GameTeamSchedule> Standard { get; set; }

    }

    public class TeamSchedule
    {

        [JsonPropertyName("league")]
        public LeagueTeamSchedule League { get; set; }
    }


}

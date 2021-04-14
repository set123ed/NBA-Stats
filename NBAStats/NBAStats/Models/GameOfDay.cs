using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

namespace NBAStats.Models
{

    public class Period : INotifyPropertyChanged
    {
        [JsonPropertyName("current")]
        public int CurrentPeriod { get; set; }

        [JsonPropertyName("isHalftime")]
        public bool IsHalftime { get; set; }

        [JsonPropertyName("isEndOfPeriod")]
        public bool IsEndOfPeriod { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class TeamScoreboard : INotifyPropertyChanged
    {
        [JsonPropertyName("teamId")]
        public string TeamId { get; set; }

        [JsonPropertyName("triCode")]
        public string TriCode { get; set; }

        [JsonPropertyName("win")]
        public string Win { get; set; }

        [JsonPropertyName("loss")]
        public string Loss { get; set; }

        [JsonPropertyName("seriesWin")]
        public string SeriesWin { get; set; }

        [JsonPropertyName("seriesLoss")]
        public string SeriesLoss { get; set; }

        [JsonPropertyName("score")]
        public string Score { get; set; }
        public string TeamLogo => $"logo{TeamId}.png";
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Game : INotifyPropertyChanged
    {

        [JsonPropertyName("seasonStageId")]
        public int SeasonStageId { get; set; }

        [JsonPropertyName("gameId")]
        public string GameId { get; set; }

        [JsonPropertyName("isGameActivated")]
        public bool IsGameActivated { get; set; }

        [JsonPropertyName("startTimeEastern")]
        public string StartTimeEastern { get; set; }

        [JsonPropertyName("startDateEastern")]
        public string StartDateEastern { get; set; }

        [JsonPropertyName("clock")]
        public string Clock { get; set; }

        [JsonPropertyName("period")]
        public Period Period { get; set; }

        [JsonPropertyName("vTeam")]
        public TeamScoreboard VTeam { get; set; }

        [JsonPropertyName("hTeam")]
        public TeamScoreboard HTeam { get; set; }

        public string ScoreOrTime { get; set; }

        public string TimePeriodHalftime { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class GameOfDay : INotifyPropertyChanged
    {

        [JsonPropertyName("games")]
        public IList<Game> Games { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

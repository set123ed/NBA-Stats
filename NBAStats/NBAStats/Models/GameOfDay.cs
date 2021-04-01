using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

namespace NBAStats.Models
{



    public class GameDuration : INotifyPropertyChanged
    {

        [JsonPropertyName("hours")]
        public string Hours { get; set; }

        [JsonPropertyName("minutes")]
        public string Minutes { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Period : INotifyPropertyChanged
    {

        [JsonPropertyName("current")]
        public int Current { get; set; }

        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("maxRegular")]
        public int MaxRegular { get; set; }

        [JsonPropertyName("isHalftime")]
        public bool IsHalftime { get; set; }

        [JsonPropertyName("isEndOfPeriod")]
        public bool IsEndOfPeriod { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class VTeam : INotifyPropertyChanged
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

        [JsonPropertyName("linescore")]
        public IList<object> Linescore { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class HTeam : INotifyPropertyChanged
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

        [JsonPropertyName("linescore")]
        public IList<object> Linescore { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }


public class Game : INotifyPropertyChanged
    {

    [JsonPropertyName("seasonStageId")]
    public int SeasonStageId { get; set; }

    [JsonPropertyName("seasonYear")]
    public string SeasonYear { get; set; }

    [JsonPropertyName("leagueName")]
    public string LeagueName { get; set; }

    [JsonPropertyName("gameId")]
    public string GameId { get; set; }


    [JsonPropertyName("isGameActivated")]
    public bool IsGameActivated { get; set; }

    [JsonPropertyName("statusNum")]
    public int StatusNum { get; set; }

    [JsonPropertyName("extendedStatusNum")]
    public int ExtendedStatusNum { get; set; }

    [JsonPropertyName("startTimeEastern")]
    public string StartTimeEastern { get; set; }

    [JsonPropertyName("startTimeUTC")]
    public DateTime StartTimeUTC { get; set; }

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

    [JsonPropertyName("gameUrlCode")]
    public string GameUrlCode { get; set; }

    [JsonPropertyName("clock")]
    public string Clock { get; set; }

    [JsonPropertyName("isBuzzerBeater")]
    public bool IsBuzzerBeater { get; set; }

    [JsonPropertyName("isPreviewArticleAvail")]
    public bool IsPreviewArticleAvail { get; set; }

    [JsonPropertyName("isRecapArticleAvail")]
    public bool IsRecapArticleAvail { get; set; }

    [JsonPropertyName("attendance")]
    public string Attendance { get; set; }

    [JsonPropertyName("hasGameBookPdf")]
    public bool HasGameBookPdf { get; set; }

    [JsonPropertyName("isStartTimeTBD")]
    public bool IsStartTimeTBD { get; set; }

    [JsonPropertyName("isNeutralVenue")]
    public bool IsNeutralVenue { get; set; }

    [JsonPropertyName("gameDuration")]
    public GameDuration GameDuration { get; set; }

    [JsonPropertyName("period")]
    public Period Period { get; set; }

    [JsonPropertyName("vTeam")]
    public VTeam VTeam { get; set; }

    [JsonPropertyName("hTeam")]
    public HTeam HTeam { get; set; }

        public string ScoreOrTime { get; set; }

        public string TimePeriodHalftime { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

public class GameOfDay : INotifyPropertyChanged
{

    [JsonPropertyName("numGames")]
    public int NumGames { get; set; }

    [JsonPropertyName("games")]
    public IList<Game> Games { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }


}

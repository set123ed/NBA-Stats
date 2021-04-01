using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json.Serialization;

namespace NBAStats.Models
{


    public class TeamSitesOnly
    {

        [JsonPropertyName("teamKey")]
        public string TeamKey { get; set; }

        [JsonPropertyName("teamName")]
        public string TeamName { get; set; }

        [JsonPropertyName("teamCode")]
        public string TeamCode { get; set; }

        [JsonPropertyName("teamNickname")]
        public string TeamNickname { get; set; }

        [JsonPropertyName("teamTricode")]
        public string TeamTricode { get; set; }

        [JsonPropertyName("clinchedConference")]
        public string ClinchedConference { get; set; }

        [JsonPropertyName("clinchedDivision")]
        public string ClinchedDivision { get; set; }

        [JsonPropertyName("clinchedPlayoffs")]
        public string ClinchedPlayoffs { get; set; }

        [JsonPropertyName("streakText")]
        public string StreakText { get; set; }
    }

    //public class SortKey
    //{

    //    [JsonPropertyName("defaultOrder")]
    //    public int DefaultOrder { get; set; }

    //    [JsonPropertyName("nickname")]
    //    public int Nickname { get; set; }

    //    [JsonPropertyName("win")]
    //    public int Win { get; set; }

    //    [JsonPropertyName("loss")]
    //    public int Loss { get; set; }

    //    [JsonPropertyName("winPct")]
    //    public int WinPct { get; set; }

    //    [JsonPropertyName("gamesBehind")]
    //    public int GamesBehind { get; set; }

    //    [JsonPropertyName("confWinLoss")]
    //    public int ConfWinLoss { get; set; }

    //    [JsonPropertyName("divWinLoss")]
    //    public int DivWinLoss { get; set; }

    //    [JsonPropertyName("homeWinLoss")]
    //    public int HomeWinLoss { get; set; }

    //    [JsonPropertyName("awayWinLoss")]
    //    public int AwayWinLoss { get; set; }

    //    [JsonPropertyName("lastTenWinLoss")]
    //    public int LastTenWinLoss { get; set; }

    //    [JsonPropertyName("streak")]
    //    public int Streak { get; set; }
    //}

    public class TeamStanding
    {
        public int Rank { get; set; }
        public string FullName { get; set; }

        [JsonPropertyName("teamId")]
        public string TeamId { get; set; }

        [JsonPropertyName("win")]
        public string Win { get; set; }

        [JsonPropertyName("loss")]
        public string Loss { get; set; }

        [JsonPropertyName("winPct")]
        public string WinPct { get; set; }

        [JsonPropertyName("winPctV2")]
        public string WinPctV2 { get; set; }

        [JsonPropertyName("lossPct")]
        public string LossPct { get; set; }

        [JsonPropertyName("lossPctV2")]
        public string LossPctV2 { get; set; }

        [JsonPropertyName("gamesBehind")]
        public string GamesBehind { get; set; }

        [JsonPropertyName("divGamesBehind")]
        public string DivGamesBehind { get; set; }

        [JsonPropertyName("clinchedPlayoffsCode")]
        public string ClinchedPlayoffsCode { get; set; }

        [JsonPropertyName("clinchedPlayoffsCodeV2")]
        public string ClinchedPlayoffsCodeV2 { get; set; }

        [JsonPropertyName("confRank")]
        public string ConfRank { get; set; }

        [JsonPropertyName("confWin")]
        public string ConfWin { get; set; }

        [JsonPropertyName("confLoss")]
        public string ConfLoss { get; set; }

        [JsonPropertyName("divWin")]
        public string DivWin { get; set; }

        [JsonPropertyName("divLoss")]
        public string DivLoss { get; set; }

        [JsonPropertyName("homeWin")]
        public string HomeWin { get; set; }

        [JsonPropertyName("homeLoss")]
        public string HomeLoss { get; set; }

        [JsonPropertyName("awayWin")]
        public string AwayWin { get; set; }

        [JsonPropertyName("awayLoss")]
        public string AwayLoss { get; set; }

        [JsonPropertyName("lastTenWin")]
        public string LastTenWin { get; set; }

        [JsonPropertyName("lastTenLoss")]
        public string LastTenLoss { get; set; }

        [JsonPropertyName("streak")]
        public string Streak { get; set; }

        [JsonPropertyName("divRank")]
        public string DivRank { get; set; }

        [JsonPropertyName("isWinStreak")]
        public bool IsWinStreak { get; set; }

        [JsonPropertyName("teamSitesOnly")]
        public TeamSitesOnly TeamSitesOnly { get; set; }

        [JsonPropertyName("tieBreakerPts")]
        public string TieBreakerPts { get; set; }

        public string L10 { get; set; }
        public string Home { get; set; }
        public string Road { get; set; }

    }

    public class Standard
    {

        [JsonPropertyName("seasonYear")]
        public int SeasonYear { get; set; }

        [JsonPropertyName("seasonStageId")]
        public int SeasonStageId { get; set; }

        [JsonPropertyName("teams")]
        public ObservableCollection<TeamStanding> Teams { get; set; }
    }

    public class StandingPerConference : ObservableCollection<TeamStanding>
    {
        public StandingPerConference(string conference)
        {
            Conference = conference;
        }

        public string Conference { get; }

    }

    public class League
    {

        [JsonPropertyName("standard")]
        public Standard Standard { get; set; }

    }

    public class Standing
    {


        [JsonPropertyName("league")]
        public League League { get; set; }
    }


}

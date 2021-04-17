using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NBAStats.Models
{
    public class TotalTeamStatsBoxScore
    {

        [JsonPropertyName("points")]
        public string Points { get; set; }

        [JsonPropertyName("fgm")]
        public string Fgm { get; set; }

        [JsonPropertyName("fga")]
        public string Fga { get; set; }

        [JsonPropertyName("fgp")]
        public string Fgp { get; set; }

        [JsonPropertyName("ftm")]
        public string Ftm { get; set; }

        [JsonPropertyName("fta")]
        public string Fta { get; set; }

        [JsonPropertyName("ftp")]
        public string Ftp { get; set; }

        [JsonPropertyName("tpm")]
        public string Tpm { get; set; }

        [JsonPropertyName("tpa")]
        public string Tpa { get; set; }

        [JsonPropertyName("tpp")]
        public string Tpp { get; set; }

        [JsonPropertyName("offReb")]
        public string OffReb { get; set; }

        [JsonPropertyName("defReb")]
        public string DefReb { get; set; }

        [JsonPropertyName("totReb")]
        public string TotReb { get; set; }

        [JsonPropertyName("assists")]
        public string Assists { get; set; }

        [JsonPropertyName("pFouls")]
        public string PFouls { get; set; }

        [JsonPropertyName("steals")]
        public string Steals { get; set; }

        [JsonPropertyName("turnovers")]
        public string Turnovers { get; set; }

        [JsonPropertyName("blocks")]
        public string Blocks { get; set; }

        [JsonPropertyName("plusMinus")]
        public string PlusMinus { get; set; }

        [JsonPropertyName("min")]
        public string Min { get; set; }

        [JsonPropertyName("short_timeout_remaining")]
        public string ShortTimeoutRemaining { get; set; }

        [JsonPropertyName("full_timeout_remaining")]
        public string FullTimeoutRemaining { get; set; }

        [JsonPropertyName("team_fouls")]
        public string TeamFouls { get; set; }
    }

    public class PlayerBoxScore
    {

        [JsonPropertyName("personId")]
        public string PersonId { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
    }

    public class StatBoxScore
    {

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("players")]
        public IList<PlayerBoxScore> Players { get; set; }
    }


    public class LeadersBoxScore
    {

        [JsonPropertyName("points")]
        public StatBoxScore Points { get; set; }

        [JsonPropertyName("rebounds")]
        public StatBoxScore Rebounds { get; set; }

        [JsonPropertyName("assists")]
        public StatBoxScore Assists { get; set; }
    }

    public class TeamBoxScore
    {

        [JsonPropertyName("fastBreakPoints")]
        public string FastBreakPoints { get; set; }

        [JsonPropertyName("pointsInPaint")]
        public string PointsInPaint { get; set; }

        [JsonPropertyName("biggestLead")]
        public string BiggestLead { get; set; }

        [JsonPropertyName("secondChancePoints")]
        public string SecondChancePoints { get; set; }

        [JsonPropertyName("pointsOffTurnovers")]
        public string PointsOffTurnovers { get; set; }

        [JsonPropertyName("longestRun")]
        public string LongestRun { get; set; }

        [JsonPropertyName("totals")]
        public TotalTeamStatsBoxScore Totals { get; set; }

        [JsonPropertyName("leaders")]
        public LeadersBoxScore Leaders { get; set; }
    }

    public class LinescoreBoxScore
    {

        [JsonPropertyName("score")]
        public string Score { get; set; }
    }

    public class TeamInfoBoxScore
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
        public IList<LinescoreBoxScore> Linescore { get; set; }
        public string TeamLogo => $"{StringConstants.Logo}{TeamId}.png";
    }

    public class BasicGameData
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

        [JsonPropertyName("endTimeUTC")]
        public DateTime EndTimeUTC { get; set; }

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

        [JsonPropertyName("period")]
        public Period Period { get; set; }

        [JsonPropertyName("vTeam")]
        public TeamInfoBoxScore VTeam { get; set; }

        [JsonPropertyName("hTeam")]
        public TeamInfoBoxScore HTeam { get; set; }

    }

    public class ActivePlayerBoxScore
    {
        public string FullName { get; set; }
        [JsonPropertyName("personId")]
        public string PersonId { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("jersey")]
        public string Jersey { get; set; }

        [JsonPropertyName("teamId")]
        public string TeamId { get; set; }

        [JsonPropertyName("isOnCourt")]
        public bool IsOnCourt { get; set; }

        [JsonPropertyName("points")]
        public string Points { get; set; }

        [JsonPropertyName("pos")]
        public string Pos { get; set; }

        [JsonPropertyName("position_full")]
        public string PositionFull { get; set; }

        [JsonPropertyName("player_code")]
        public string PlayerCode { get; set; }

        [JsonPropertyName("min")]
        public string Min { get; set; }

        [JsonPropertyName("fgm")]
        public string Fgm { get; set; }

        [JsonPropertyName("fga")]
        public string Fga { get; set; }

        [JsonPropertyName("fgp")]
        public string Fgp { get; set; }

        [JsonPropertyName("ftm")]
        public string Ftm { get; set; }

        [JsonPropertyName("fta")]
        public string Fta { get; set; }

        [JsonPropertyName("ftp")]
        public string Ftp { get; set; }

        [JsonPropertyName("tpm")]
        public string Tpm { get; set; }

        [JsonPropertyName("tpa")]
        public string Tpa { get; set; }

        [JsonPropertyName("tpp")]
        public string Tpp { get; set; }

        [JsonPropertyName("offReb")]
        public string OffReb { get; set; }

        [JsonPropertyName("defReb")]
        public string DefReb { get; set; }

        [JsonPropertyName("totReb")]
        public string TotReb { get; set; }

        [JsonPropertyName("assists")]
        public string Assists { get; set; }

        [JsonPropertyName("pFouls")]
        public string PFouls { get; set; }

        [JsonPropertyName("steals")]
        public string Steals { get; set; }

        [JsonPropertyName("turnovers")]
        public string Turnovers { get; set; }

        [JsonPropertyName("blocks")]
        public string Blocks { get; set; }

        [JsonPropertyName("plusMinus")]
        public string PlusMinus { get; set; }

        [JsonPropertyName("dnp")]
        public string Dnp { get; set; }
        public bool IsFavorite { get; set; } = false;
    }

    public class StatsBoxScore
    {
        [JsonPropertyName("vTeam")]
        public TeamBoxScore VTeam { get; set; }

        [JsonPropertyName("hTeam")]
        public TeamBoxScore HTeam { get; set; }

        [JsonPropertyName("activePlayers")]
        public IList<ActivePlayerBoxScore> ActivePlayers { get; set; }
    }

    public class BoxScore
    {


        [JsonPropertyName("basicGameData")]
        public BasicGameData BasicGameData { get; set; }

        [JsonPropertyName("stats")]
        public StatsBoxScore Stats { get; set; }
    }


}

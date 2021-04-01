using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NBAStats.Models
{
    public class TotalsBoxScore
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

    public class PointsBoxScore
    {

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("players")]
        public IList<PlayerBoxScore> Players { get; set; }
    }

    public class ReboundsBoxScore
    {

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("players")]
        public IList<PlayerBoxScore> Players { get; set; }
    }

    public class AssistsBoxScore
    {

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("players")]
        public IList<PlayerBoxScore> Players { get; set; }
    }

    public class LeadersBoxScore
    {

        [JsonPropertyName("points")]
        public PointsBoxScore Points { get; set; }

        [JsonPropertyName("rebounds")]
        public ReboundsBoxScore Rebounds { get; set; }

        [JsonPropertyName("assists")]
        public AssistsBoxScore Assists { get; set; }
    }

    public class VTeamTotalBoxScore
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
        public TotalsBoxScore Totals { get; set; }

        [JsonPropertyName("leaders")]
        public LeadersBoxScore Leaders { get; set; }
    }

    public class HTeamTotalBoxScore
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
        public TotalsBoxScore Totals { get; set; }

        [JsonPropertyName("leaders")]
        public LeadersBoxScore Leaders { get; set; }
    }
    public class GameDurationBoxScore
    {

        [JsonPropertyName("hours")]
        public string Hours { get; set; }

        [JsonPropertyName("minutes")]
        public string Minutes { get; set; }
    }

    public class PeriodBoxScore
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
    }

    public class LinescoreBoxScore
    {

        [JsonPropertyName("score")]
        public string Score { get; set; }
    }

    public class VTeamBoxScore
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
    }


    public class HTeamBoxScore
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

        [JsonPropertyName("gameDuration")]
        public GameDurationBoxScore GameDuration { get; set; }

        [JsonPropertyName("period")]
        public PeriodBoxScore Period { get; set; }

        [JsonPropertyName("vTeam")]
        public VTeam VTeamBoxScore { get; set; }

        [JsonPropertyName("hTeam")]
        public HTeamBoxScore HTeam { get; set; }

    }

    public class PreviousMatchup
    {

        [JsonPropertyName("gameId")]
        public string GameId { get; set; }

        [JsonPropertyName("gameDate")]
        public string GameDate { get; set; }
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
    }

    public class StatsBoxScore
    {

        [JsonPropertyName("timesTied")]
        public string TimesTied { get; set; }

        [JsonPropertyName("leadChanges")]
        public string LeadChanges { get; set; }

        [JsonPropertyName("vTeam")]
        public VTeamTotalBoxScore VTeam { get; set; }

        [JsonPropertyName("hTeam")]
        public HTeamTotalBoxScore HTeam { get; set; }

        [JsonPropertyName("activePlayers")]
        public IList<ActivePlayerBoxScore> ActivePlayers { get; set; }
    }

    public class BoxScore
    {


        [JsonPropertyName("basicGameData")]
        public BasicGameData BasicGameData { get; set; }

        [JsonPropertyName("previousMatchup")]
        public PreviousMatchup PreviousMatchup { get; set; }

        [JsonPropertyName("stats")]
        public StatsBoxScore Stats { get; set; }
    }


}

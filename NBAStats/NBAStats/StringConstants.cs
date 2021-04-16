using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats
{
    public static class StringConstants
    {
        public const string ChoosePlayer = "Choose a player";
        public const string Cancel = "Cancel";
        public const string East = "East";
        public const string West = "West";
        public const string PtsStatParameter= "PTS";
        public const string AstStatParameter= "AST";
        public const string RebStatParameter= "REB";
        public const string StlStatParameter= "STL";
        public const string BlkStatParameter= "BLK";
        public const string Fg3mStatParameter= "FG3M";
        public const string PointsPerGame = "Points per game";
        public const string AssistsPerGame = "Assists per game";
        public const string ReboundsPerGame = "Rebounds per game";
        public const string StealsPerGame = "Steals per game";
        public const string BlocksPerGame = "Blocks per game";
        public const string TpmgPerGame = "Three points made per game";
        public const string TppPerGame = "3P% per game";
        public const string FgpPerGame = "FG% per game";
        public const string FtpPerGame = "FT% per game";
        public const string TurnoversPerGame = "Turnovers per game";
        
        
        public const string PreSeasonFilter = "preseason";
        public const string RegularSeasonFilter = "regularseason";
        public const string AllStarSeasonFilter = "allstar";
        public const string PlayInSeasonFilter = "playin";

        public const string Win = "WIN";
        public const string Loss = "LOSS";

        public const string ScoreInZero= "0 - 0";
        public const string QuarterGame = "QUARTER";
        public const string OTGame = "OT";
        public const string HalftimeGame = "HALFTIME";
        public const string EndOfPeriod = "END OF";
        public const string FinalGame = "FINAL";
        public const string LeftTime = "LEFT";

        public const string Logo = "logo";

        public static string GetActualTeamInfo(string teamTricode)
        {
            return $"In {teamTricode} since: ";
        }
        
        public static string GetNextGames(int nextGamesCount)
        {
            return $"Next {nextGamesCount} games";
        }
        
        public static string GetLastGames(int lastGamesCount)
        {
            return $"Last {lastGamesCount} games played";
        }
        
        public static string GetGamesBehindFirst(string gamesBehind, string conferenceName)
        {
            return $"{gamesBehind} games behind of {conferenceName}'s first place";
        }
        
        public static string GetConferenceRank(string conferenceRank, string conferenceName)
        {
            return $"{conferenceRank} place of {conferenceName} conference";
        }



    }
}

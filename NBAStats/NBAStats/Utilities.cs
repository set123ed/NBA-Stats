using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats
{
    public static class Utilities
    {
        public static string GetScoreOrTime(string vTeamScore, string hTeamScore, string gameStartTime)
        {
            if (!string.IsNullOrEmpty(hTeamScore) && $"{vTeamScore} - {hTeamScore}" != StringConstants.ScoreInZero)
            {
                return $"{vTeamScore} - {hTeamScore}";
            }
            else
            {
                return gameStartTime;
            }
        }

        public static string GetTimePeriod(string vTeamScore, string hTeamScore, int currentPeriod, bool isHalftime, bool isEndOfPeriod, bool isGameActivated, string clock)
        {
            if (!string.IsNullOrEmpty(hTeamScore) && $"{vTeamScore} - {hTeamScore}" != StringConstants.ScoreInZero)
            {
                if (isHalftime)
                {
                    return StringConstants.HalftimeGame;
                }
                else if (isEndOfPeriod && currentPeriod <= 4)
                {
                    return $" {currentPeriod} {StringConstants.QuarterGame}";
                }
                else if (isEndOfPeriod && currentPeriod > 4)
                {
                    return $"{StringConstants.EndOfPeriod} {currentPeriod - 4} {StringConstants.OTGame}";
                }
                else if (!isGameActivated)
                {
                    return StringConstants.FinalGame;
                }
                else if (currentPeriod <= 4)
                {
                    return $"{currentPeriod} {StringConstants.QuarterGame} - {clock} {StringConstants.LeftTime}";
                }
                else if (currentPeriod > 4)
                {
                    return $"{currentPeriod - 4} {StringConstants.OTGame} - {clock} {StringConstants.LeftTime}";
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }
    }
}

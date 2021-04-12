using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats
{
    public static class Utilities
    {
        public static string GetScoreOrTime(string vTeamScore, string hTeamScore, string gameStartTime)
        {
            if (!string.IsNullOrEmpty(hTeamScore) && $"{vTeamScore} - {hTeamScore}" != "0 - 0")
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
            if (!string.IsNullOrEmpty(hTeamScore) && $"{vTeamScore} - {hTeamScore}" != "0 - 0")
            {
                if (isHalftime)
                {
                    return "HALFTIME";
                }
                else if (isEndOfPeriod && currentPeriod <= 4)
                {
                    return $"END OF {currentPeriod} QUARTER";
                }
                else if (isEndOfPeriod && currentPeriod > 4)
                {
                    return $"END OF {currentPeriod - 4} OT";
                }
                else if (!isGameActivated)
                {
                    return "FINAL";
                }
                else if (currentPeriod <= 4)
                {
                    return $"{currentPeriod} QUARTER - {clock} LEFT";
                }
                else if (currentPeriod > 4)
                {
                    return $"{currentPeriod - 4} OT - {clock} LEFT";
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

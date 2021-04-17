using NBAStats.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
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

        public static ObservableCollection<Player> SetFavoritesPlayers(IEnumerable<Player> normalPlayers, IEnumerable<FavoritesPlayer> favoritesPlayers)
        {
            var favoritesPlayersId = favoritesPlayers.Select(p => p.PlayerId).ToList();

            foreach (Player player in normalPlayers)
            {
                player.IsFavorite = favoritesPlayersId.Contains(player.PersonId);
            }

            ObservableCollection<Player> allPlayersWithFavorites = new ObservableCollection<Player>(normalPlayers.OrderByDescending(p => p.IsFavorite).ThenBy(p=> p.FullName));
            return allPlayersWithFavorites;
        }

        public static ObservableCollection<Team> SetFavoritesTeam(IEnumerable<Team> normalTeams, IEnumerable<FavoritesTeam> favoritesTeams)
        {
            var favoritesTeamsId = favoritesTeams.Select(t => t.TeamId).ToList();

            foreach (Team team in normalTeams)
            {
                team.IsFavorite = favoritesTeamsId.Contains(team.TeamId);
            }

            ObservableCollection<Team> allTeamsWithFavorites = new ObservableCollection<Team>(normalTeams.OrderByDescending(p => p.IsFavorite).ThenBy(p=> p.FullName));

            return allTeamsWithFavorites;
        }

        public static ObservableCollection<ActivePlayerBoxScore> SetBoxScoresFavoritesPlayers(IEnumerable<ActivePlayerBoxScore> players, IEnumerable<FavoritesPlayer> favoritesPlayers)
        {
            var favoritesPlayersId = favoritesPlayers.Select(p => p.PlayerId).ToList();
            List<ActivePlayerBoxScore> playersThatPlay = new List<ActivePlayerBoxScore>(players);
            List<ActivePlayerBoxScore> playersThatNotPlay = new List<ActivePlayerBoxScore>(playersThatPlay.Where(p => string.IsNullOrEmpty(p.Min) || p.Min == "0"));
            playersThatPlay.RemoveAll(p => string.IsNullOrEmpty(p.Min) || p.Min == "0");

            foreach (ActivePlayerBoxScore player in playersThatPlay)
            {
                player.IsFavorite = favoritesPlayersId.Contains(player.PersonId);
            }

            playersThatPlay = new List<ActivePlayerBoxScore>(playersThatPlay.OrderByDescending(p => p.IsFavorite).ThenByDescending(p => DateTime.ParseExact(p.Min, "m:s", CultureInfo.InvariantCulture)).ThenByDescending(p => p.Points));
            playersThatPlay.AddRange(playersThatNotPlay);
            ObservableCollection<ActivePlayerBoxScore> allPlayersBoxScoreWithFav = new ObservableCollection<ActivePlayerBoxScore>(playersThatPlay);
            
            return allPlayersBoxScoreWithFav;
        }

        public static ObservableCollection<Game> SetFavoritesTeamsOnGame(IEnumerable<Game> games, IEnumerable<FavoritesTeam> favoritesTeams)
        {
            var favoritesTeamsId = favoritesTeams.Select(team => team.TeamId).ToList();

            foreach (Game game in games)
            {
                if (favoritesTeamsId.Contains(game.VTeam.TeamId))
                {
                    game.VTeam.IsFavorite = true;
                    ++game.FavoritesTeamOnGame;
                }
                else if (favoritesTeamsId.Contains(game.HTeam.TeamId))
                {
                    game.HTeam.IsFavorite = true;
                    ++game.FavoritesTeamOnGame;
                }
            }

            return new ObservableCollection<Game>(games.OrderByDescending(game => game.FavoritesTeamOnGame));
            
        }

        public static ObservableCollection<TeamStanding> SetFavoriteTeamsOnStanding(IEnumerable<TeamStanding> standing, IEnumerable<FavoritesTeam> favoritesTeams)
        {
            var favoritesTeamsId = favoritesTeams.Select(team => team.TeamId).ToList();

            foreach (TeamStanding teamStanding in standing)
            {
                teamStanding.IsFavorite = favoritesTeamsId.Contains(teamStanding.TeamId);
            }

            return new ObservableCollection<TeamStanding>(standing);
        }

        public static ObservableCollection<PlayerRegularStats> SetFavoritesRegularPlayer(IEnumerable<PlayerRegularStats> playerRegularStats, IEnumerable<FavoritesPlayer> favoritesPlayers)
        {
            var favoritesPlayersId = favoritesPlayers.Select(p => p.PlayerId).ToList();

            foreach (PlayerRegularStats player in playerRegularStats)
            {
                player.IsFavorite = favoritesPlayersId.Contains(player.PlayerId);
            }

            return new ObservableCollection<PlayerRegularStats>(playerRegularStats);
        }
        
        public static ObservableCollection<LeaderStatsPlayerCollection> SetFavoritesPlayersLeaderStats (IEnumerable<LeaderStatsPlayerCollection> playersPerStat, IEnumerable<FavoritesPlayer> favoritesPlayers)
        {
            var favoritesPlayersId = favoritesPlayers.Select(p => p.PlayerId).ToList();

            foreach (ObservableCollection<LeadersStatsPlayer> players in playersPerStat)
            {
                foreach (LeadersStatsPlayer player in players)
                {
                    player.IsFavorite = favoritesPlayersId.Contains(player.PlayerId);
                }
            }

            return new ObservableCollection<LeaderStatsPlayerCollection>(playersPerStat);
        }

        public static ObservableCollection<LeaderStatsTeamCollection> SetFavoritesTeamsLeaderStats (IEnumerable<LeaderStatsTeamCollection> teamsPerStat, IEnumerable<FavoritesTeam> favoritesTeams)
        {
            var favoritesTeamsId = favoritesTeams.Select(team => team.TeamId).ToList();

            foreach (ObservableCollection<LeadersStatsTeam> teams in teamsPerStat)
            {
                foreach (LeadersStatsTeam team in teams)
                {
                    team.IsFavorite = favoritesTeamsId.Contains(team.TeamId);
                }
            }

            return new ObservableCollection<LeaderStatsTeamCollection>(teamsPerStat);
        }

        public static ObservableCollection<GameScheduleCollection> SetFavoritesTeamsOnSchedule (IEnumerable<GameScheduleCollection> schedule, IEnumerable<FavoritesTeam> favoritesTeams)
        {
            var favoritesTeamsId = favoritesTeams.Select(team => team.TeamId).ToList();

            foreach (ObservableCollection<GameTeamSchedule> games in schedule)
            {
                foreach (GameTeamSchedule game in games)
                {
                    game.VTeam.IsFavorite = favoritesTeamsId.Contains(game.VTeam.TeamId);
                    game.HTeam.IsFavorite = favoritesTeamsId.Contains(game.HTeam.TeamId);
                }
            }

            return new ObservableCollection<GameScheduleCollection>(schedule);
        }
    }
}

using NBAStats.Models;
using NBAStats.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBAStats.ViewModels
{
    class TeamProfileViewModel : BaseViewModel,IInitialize
    {
        public ObservableCollection<GameScheduleCollection> GameSchedule { get; set; } = new ObservableCollection<GameScheduleCollection>();
        public ObservableCollection<Player> Roster { get; set; }
        public ObservableCollection<TeamLeadersPlayers> TeamLeaders { get; set; } = new ObservableCollection<TeamLeadersPlayers>();

        public Team Team { get; set; }

        private List<Player> playersList = new List<Player>();

        public TeamProfileViewModel(INavigationService navigationService, INbaApiService nbaApiService) : base(navigationService, nbaApiService)
        {
        }

        public async void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("team", out Team team) && parameters.TryGetValue("players", out List<Player> players))
            {
                Team = team;
                playersList = players;

                await GetTeamInfo();
                await GetTeamLeaders();
            }


        }

        private async Task GetTeamInfo()
        {
            var teamSchedule = await NbaApiService.GetTeamSchedule("2020",Team.UrlName);
            if (teamSchedule.GetType().Name == "TeamSchedule")
            {
                if (teamSchedule != null)
                {
                    ObservableCollection<Player> roster = new ObservableCollection<Player>(playersList.Where(player => player.TeamId == Team.TeamId));


                    foreach (Player player in playersList)
                    {
                        if (player.TeamId == Team.TeamId)
                        {
                            player.FullName = $"{player.FirstName} {player.LastName}";

                            roster.Add(player);
                        }
                    }

                    Roster = roster;

                    int lastGamePlayedIndex = teamSchedule.League.LastStandardGamePlayedIndex;

                    int lastGamesCount = 0;

                    if ((lastGamePlayedIndex - 5) >= 0)
                    {
                        lastGamesCount = 5;
                    }
                    else
                    {
                        lastGamesCount = lastGamePlayedIndex;
                    }

                    GameScheduleCollection lastGamesPlayed = new GameScheduleCollection($"Last {lastGamesCount} games played");

                    ObservableCollection<GameTeamSchedule> lastGames = new ObservableCollection<GameTeamSchedule>(teamSchedule.League.Standard.ToList().GetRange(lastGamePlayedIndex - lastGamesCount + 1, lastGamesCount));
                    foreach (GameTeamSchedule game in lastGames)
                    {
                        string gameUrl = game.GameUrlCode;
                        string vTeamTricode = gameUrl.Substring(gameUrl.IndexOf('/') + 1, 3);
                        string hTeamTricode = gameUrl.Substring(gameUrl.IndexOf('/') + 4, 3);

                        game.VTeam.Tricode = vTeamTricode;
                        game.HTeam.Tricode = hTeamTricode;

                        if (string.IsNullOrEmpty(game.HTeam.Score))
                        {
                            game.ScoreOrTime = game.StartTimeEastern;
                        }
                        else
                        {
                            game.ScoreOrTime = $"{game.VTeam.Score} - {game.HTeam.Score}";
                        }

                        game.SeasonStage = Config.SeasonStages.First(stage => stage.Id == game.SeasonStageId).Stage;

                        lastGamesPlayed.Add(game);
                    }

                    int nextGamesCount = 0;

                    if ((teamSchedule.League.LastStandardGamePlayedIndex + 5) <= teamSchedule.League.Standard.Count)
                    {
                        nextGamesCount = 5;
                    }
                    else
                    {
                        nextGamesCount = teamSchedule.League.Standard.Count - teamSchedule.League.LastStandardGamePlayedIndex;
                    }

                    GameScheduleCollection nextGamesToPlay = new GameScheduleCollection($"Next {nextGamesCount} games");

                    ObservableCollection<GameTeamSchedule> nextGames = new ObservableCollection<GameTeamSchedule>(teamSchedule.League.Standard.ToList().GetRange(lastGamePlayedIndex +1, nextGamesCount));
                    foreach (GameTeamSchedule game in nextGames)
                    {
                        string gameUrl = game.GameUrlCode;
                        string vTeamTricode = gameUrl.Substring(gameUrl.IndexOf('/') + 1, 3);
                        string hTeamTricode = gameUrl.Substring(gameUrl.IndexOf('/') + 4, 3);

                        game.VTeam.Tricode = vTeamTricode;
                        game.HTeam.Tricode = hTeamTricode;

                        if (string.IsNullOrEmpty(game.HTeam.Score))
                        {
                            game.ScoreOrTime = game.StartTimeEastern;
                        }
                        else
                        {
                            game.ScoreOrTime = $"{game.VTeam.Score} - {game.HTeam.Score}";
                        }

                        game.SeasonStage = Config.SeasonStages.First(stage => stage.Id == game.SeasonStageId).Stage;

                        nextGamesToPlay.Add(game);
                    }

                    GameSchedule.Add(lastGamesPlayed);
                    GameSchedule.Add(nextGamesToPlay);


                }
            }
        }

        public async Task GetTeamLeaders()
        {
            var teamLeaders = await NbaApiService.GetTeamLeaders("2020", Team.UrlName);

            if (teamLeaders.GetType().Name == "TeamLeaders")
            {
                if (teamLeaders != null)
                {
                    TeamLeadersPlayers playerPpg = new TeamLeadersPlayers
                    {
                        PlayerId = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Ppg[0].PersonId,
                        FullName = Roster.First(player => player.PersonId == teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Ppg[0].PersonId).FullName,
                        StatName = "Pts per game",
                        StatAvg = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Ppg[0].Value
                    };
                    TeamLeadersPlayers playerApg = new TeamLeadersPlayers
                    {
                        PlayerId = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Apg[0].PersonId,
                        FullName = Roster.First(player => player.PersonId == teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Apg[0].PersonId).FullName,
                        StatName = "Asts per game",
                        StatAvg = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Apg[0].Value
                    };
                    TeamLeadersPlayers playerRpg = new TeamLeadersPlayers
                    {
                        PlayerId = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Trpg[0].PersonId,
                        FullName = Roster.First(player => player.PersonId == teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Trpg[0].PersonId).FullName,
                        StatName = "Rebs per game",
                        StatAvg = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Trpg[0].Value
                    };
                    TeamLeadersPlayers playerSpg = new TeamLeadersPlayers
                    {
                        PlayerId = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Spg[0].PersonId,
                        FullName = Roster.First(player => player.PersonId == teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Spg[0].PersonId).FullName,
                        StatName = "Stls per game",
                        StatAvg = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Spg[0].Value
                    };
                    TeamLeadersPlayers playerBpg = new TeamLeadersPlayers
                    {
                        PlayerId = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Bpg[0].PersonId,
                        FullName = Roster.First(player => player.PersonId == teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Bpg[0].PersonId).FullName,
                        StatName = "Blks per game",
                        StatAvg = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Bpg[0].Value
                    };
                    TeamLeadersPlayers playerFgp = new TeamLeadersPlayers
                    {
                        PlayerId = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Fgp[0].PersonId,
                        FullName = Roster.First(player => player.PersonId == teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Fgp[0].PersonId).FullName,
                        StatName = "FG% per game",
                        StatAvg = Math.Round(Convert.ToDecimal(teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Fgp[0].Value) * 100, 1).ToString() + "%"
                    };
                    TeamLeadersPlayers playerFtp = new TeamLeadersPlayers
                    {
                        PlayerId = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Ftp[0].PersonId,
                        FullName = Roster.First(player => player.PersonId == teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Ftp[0].PersonId).FullName,
                        StatName = "FT% per game",
                        StatAvg = Math.Round(Convert.ToDecimal(teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Ftp[0].Value) * 100, 1).ToString() + "%"
                    };
                    TeamLeadersPlayers playerTpp = new TeamLeadersPlayers
                    {
                        PlayerId = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Tpp[0].PersonId,
                        FullName = Roster.First(player => player.PersonId == teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Tpp[0].PersonId).FullName,
                        StatName = "3P% per game",
                        StatAvg = Math.Round(Convert.ToDecimal(teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Tpp[0].Value) * 100, 1).ToString() + "%"
                    };
                    TeamLeadersPlayers playerTpg = new TeamLeadersPlayers
                    {
                        PlayerId = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Tpg[0].PersonId,
                        FullName = Roster.First(player => player.PersonId == teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Tpg[0].PersonId).FullName,
                        StatName = "Tovs per game",
                        StatAvg = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Tpg[0].Value
                    };

                    ObservableCollection<TeamLeadersPlayers> teamLeadersPlayers = new ObservableCollection<TeamLeadersPlayers>();

                    teamLeadersPlayers.Add(playerPpg);
                    teamLeadersPlayers.Add(playerApg);
                    teamLeadersPlayers.Add(playerRpg);
                    teamLeadersPlayers.Add(playerSpg);
                    teamLeadersPlayers.Add(playerBpg);
                    teamLeadersPlayers.Add(playerFgp);
                    teamLeadersPlayers.Add(playerTpp);
                    teamLeadersPlayers.Add(playerFtp);
                    teamLeadersPlayers.Add(playerTpg);

                    TeamLeaders = teamLeadersPlayers;
                }
            }
        }

        
    }
}

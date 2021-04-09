using NBAStats.Constants;
using NBAStats.Models;
using NBAStats.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NBAStats.ViewModels
{
    public class TeamProfileViewModel : BaseViewModel,IInitialize
    {
        public ObservableCollection<GameScheduleCollection> GameSchedule { get; set; } = new ObservableCollection<GameScheduleCollection>();
        public ObservableCollection<Player> Roster { get; set; }
        public TeamStats TeamRegularStats { get; set; }


        public string ConferencePlace { get; set; }
        public string TeamRecord { get; set; }
        public string GamesBehindFirstPlace { get; set; }
        public bool GamesBehindShow { get; set; } = false;

        public TeamStanding TeamStanding { get; set; }
        public ObservableCollection<TeamLeadersPlayers> TeamLeaders { get; set; } = new ObservableCollection<TeamLeadersPlayers>();

        public Team Team { get; set; }

        private List<Player> _playersList = new List<Player>();
        private List<Team> _teamList = new List<Team>();

        public ICommand GameSelectedCommand { get; }
        public ICommand SelectedPlayerCommand { get; }

        public bool IsBusy { get; set; } = true;
        public bool IsNotBusy => !IsBusy;

        public TeamProfileViewModel(INavigationService navigationService, INbaApiService nbaApiService) : base(navigationService, nbaApiService)
        {
            GameSelectedCommand = new Command<GameTeamSchedule>(OnGameSelected);
            SelectedPlayerCommand = new Command<string>(OnSelectedPlayer);
        }

        private async void OnGameSelected(GameTeamSchedule game)
        {
            var parameters = new NavigationParameters();
            parameters.Add(ParametersConstants.GameId, game.GameId);
            parameters.Add(ParametersConstants.DateGame, game.StartDateEastern);
            parameters.Add(ParametersConstants.PlayerList, _playersList);
            parameters.Add(ParametersConstants.TeamList, _teamList);

            await NavigationService.NavigateAsync(NavigationConstants.BoxScorePage, parameters);
        }

        private async void OnSelectedPlayer(string playerId)
        {
            var parameters = new NavigationParameters();
            parameters.Add(ParametersConstants.TeamList, new List<Team>(_teamList));
            parameters.Add(ParametersConstants.PlayerId, playerId);
            parameters.Add(ParametersConstants.PlayerList, new List<Player>(_playersList));

            await NavigationService.NavigateAsync(NavigationConstants.PlayerProfilePage, parameters);
        }

        public async void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(ParametersConstants.Team, out Team team) && parameters.TryGetValue(ParametersConstants.PlayerList, out List<Player> playersList) && parameters.TryGetValue(ParametersConstants.TeamList, out List<Team> teamsList))
            {
                Team = team;

                _teamList = teamsList;

                _playersList = playersList;

                await GetSeasonYearParameters();
                await GetTeamInfo();
                await GetTeamLeaders();
                await GetTeamStats();
                await GetTeamStanding();
                IsBusy = false;
            }


        }

        private async Task GetTeamStanding()
        {
            var standingApi = await NbaApiService.GetStanding();

            if (standingApi.GetType().Name == "Standing")
            {
                if (standingApi != null)
                {
                    TeamStanding = standingApi.League.Standard.Teams.First(team => team.TeamId == Team.TeamId);

                    if (TeamStanding.ConfRank != "1")
                    {
                        GamesBehindFirstPlace = $"{TeamStanding.GamesBehind} games behind of {Team.ConfName}'s first place";
                        GamesBehindShow = true;
                    }
                    else
                    {
                        GamesBehindShow = false;
                    }

                    ConferencePlace = $"{TeamStanding.ConfRank} place of {Team.ConfName} conference";
                    TeamRecord = $"{TeamStanding.Win} - {TeamStanding.Loss}";
                }
            }
        }

        private async Task GetTeamInfo()
        {
            var teamSchedule = await NbaApiService.GetTeamSchedule(_seasonYearApiData, Team.UrlName);
            if (teamSchedule.GetType().Name == "TeamSchedule")
            {
                if (teamSchedule != null)
                {
                    ObservableCollection<Player> roster = new ObservableCollection<Player>(_playersList.Where(player => player.TeamId == Team.TeamId));

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

                            if (game.IsHomeTeam)
                            {
                                if (Convert.ToDecimal(game.HTeam.Score) > Convert.ToDecimal(game.VTeam.Score))
                                {
                                    game.Result = "WIN";
                                }
                                else
                                {
                                    game.Result = "LOSS";
                                }
                            }
                            else
                            {
                                if (Convert.ToDecimal(game.VTeam.Score) > Convert.ToDecimal(game.HTeam.Score))
                                {
                                    game.Result = "WIN";
                                }
                                else
                                {
                                    game.Result = "LOSS";
                                }
                            }
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
            var teamLeaders = await NbaApiService.GetTeamLeaders(_seasonYearApiData, Team.UrlName);

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

        private async Task GetTeamStats()
        {
            var teamStat = await NbaApiService.GetTeamStats(_seasonYearApiData);

            if (teamStat.GetType().Name == "TeamStatsClass")
            {
                if (teamStat != null)
                {
                    TeamStats teamRegularStats = teamStat.LeagueTeamStats.Seasons.RegularSeason.Teams.First(team => team.TeamId == Team.TeamId);

                    teamRegularStats.Fgp.Avg = Math.Round(Convert.ToDecimal(teamRegularStats.Fgp.Avg) * 100, 1).ToString();
                    teamRegularStats.Tpp.Avg = Math.Round(Convert.ToDecimal(teamRegularStats.Tpp.Avg) * 100, 1).ToString();
                    teamRegularStats.Ftp.Avg = Math.Round(Convert.ToDecimal(teamRegularStats.Ftp.Avg) * 100, 1).ToString();

                    TeamRegularStats = teamRegularStats;

                }
            }
        }
    }
}

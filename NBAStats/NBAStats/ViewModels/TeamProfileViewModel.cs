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

        public ICommand GameSelectedCommand { get; }
        public ICommand SelectedPlayerCommand { get; }

        public bool IsBusy { get; set; } = true;
        public bool IsNotBusy => !IsBusy;

        public TeamProfileViewModel(INbaApiService nbaApiService, INavigationService navigationService, INbaDefaultInfoService nbaDefaultInfoService,IDataBaseServices baseServices) : base(navigationService, nbaApiService, nbaDefaultInfoService,baseServices)
        {
            GameSelectedCommand = new Command<GameTeamSchedule>(OnGameSelected);
            SelectedPlayerCommand = new Command<string>(OnSelectedPlayer);
        }

        private async void OnGameSelected(GameTeamSchedule game)
        {
            var parameters = new NavigationParameters();
            parameters.Add(ParametersConstants.GameId, game.GameId);
            parameters.Add(ParametersConstants.DateGame, game.StartDateEastern);

            await NavigationService.NavigateAsync(NavigationConstants.BoxScorePage, parameters);
        }

        private async void OnSelectedPlayer(string playerId)
        {
            var parameters = new NavigationParameters();
            parameters.Add(ParametersConstants.PlayerId, playerId);

            await NavigationService.NavigateAsync(NavigationConstants.PlayerProfilePage, parameters);
        }

        public async void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(ParametersConstants.Team, out Team team))
            {
                Team = team;
                await GetDefaultData();
                await GetTeamInfo();
                await GetTeamLeaders();
                await GetTeamStats();
                await GetTeamStanding();
                IsBusy = false;
            }


        }

        private async Task GetTeamStanding()
        {
            try
            {
                Standing standing = await NbaApiService.GetStanding();

                if (standing != null)
                {
                    TeamStanding = standing.League.Standard.Teams.First(team => team.TeamId == Team.TeamId);

                    if (TeamStanding.ConfRank != "1")
                    {
                        GamesBehindFirstPlace = StringConstants.GetGamesBehindFirst(TeamStanding.GamesBehind, Team.ConfName);
                        GamesBehindShow = true;
                    }
                    else
                    {
                        GamesBehindShow = false;
                    }

                    ConferencePlace = StringConstants.GetConferenceRank(TeamStanding.ConfRank, Team.ConfName);
                    TeamRecord = $"{TeamStanding.Win} - {TeamStanding.Loss}";
                }
            }
            catch (NoInternetConnectionException ex)
            {

            }

            
        }

        private async Task GetTeamInfo()
        {
            try
            {
                TeamSchedule teamSchedule = await NbaApiService.GetTeamSchedule(_seasonYearApiData, Team.UrlName);

                if (teamSchedule != null)
                {
                    ObservableCollection<Player> roster = new ObservableCollection<Player>(_playerList.Where(player => player.TeamId == Team.TeamId));

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

                    GameScheduleCollection lastGamesPlayed = new GameScheduleCollection(StringConstants.GetLastGames(lastGamesCount));

                    ObservableCollection<GameTeamSchedule> lastGames = new ObservableCollection<GameTeamSchedule>(teamSchedule.League.Standard.ToList().GetRange(lastGamePlayedIndex - lastGamesCount + 1, lastGamesCount));
                    foreach (GameTeamSchedule game in lastGames)
                    {
                        string gameUrl = game.GameUrlCode;
                        string vTeamTricode = gameUrl.Substring(gameUrl.IndexOf('/') + 1, 3);
                        string hTeamTricode = gameUrl.Substring(gameUrl.IndexOf('/') + 4, 3);

                        game.VTeam.Tricode = vTeamTricode;
                        game.HTeam.Tricode = hTeamTricode;

                        game.ScoreOrTime = Utilities.GetScoreOrTime(game.VTeam.Score, game.HTeam.Score, game.StartTimeEastern);

                        if (game.ScoreOrTime.Contains("-"))
                        {

                            if (game.IsHomeTeam)
                            {
                                if (Convert.ToDecimal(game.HTeam.Score) > Convert.ToDecimal(game.VTeam.Score))
                                {
                                    game.Result = StringConstants.Win;
                                }
                                else
                                {
                                    game.Result = StringConstants.Loss;
                                }
                            }
                            else
                            {
                                if (Convert.ToDecimal(game.VTeam.Score) > Convert.ToDecimal(game.HTeam.Score))
                                {
                                    game.Result = StringConstants.Win;
                                }
                                else
                                {
                                    game.Result = StringConstants.Loss;
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

                    GameScheduleCollection nextGamesToPlay = new GameScheduleCollection(StringConstants.GetNextGames(nextGamesCount));

                    ObservableCollection<GameTeamSchedule> nextGames = new ObservableCollection<GameTeamSchedule>(teamSchedule.League.Standard.ToList().GetRange(lastGamePlayedIndex + 1, nextGamesCount));
                    foreach (GameTeamSchedule game in nextGames)
                    {
                        string gameUrl = game.GameUrlCode;
                        string vTeamTricode = gameUrl.Substring(gameUrl.IndexOf('/') + 1, 3);
                        string hTeamTricode = gameUrl.Substring(gameUrl.IndexOf('/') + 4, 3);

                        game.VTeam.Tricode = vTeamTricode;
                        game.HTeam.Tricode = hTeamTricode;

                        game.ScoreOrTime = Utilities.GetScoreOrTime(game.VTeam.Score, game.HTeam.Score, game.StartTimeEastern);

                        game.SeasonStage = Config.SeasonStages.First(stage => stage.Id == game.SeasonStageId).Stage;

                        nextGamesToPlay.Add(game);
                    }

                    GameSchedule.Add(lastGamesPlayed);
                    GameSchedule.Add(nextGamesToPlay);


                }

            }
            catch (NoInternetConnectionException ex)
            {

            }
            
        }

        public async Task GetTeamLeaders()
        {
            try
            {
                TeamLeaders teamLeaders = await NbaApiService.GetTeamLeaders(_seasonYearApiData, Team.UrlName);

                if (teamLeaders != null)
                {
                    TeamLeadersPlayers playerPpg = new TeamLeadersPlayers
                    {
                        PlayerId = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Ppg[0].PersonId,
                        FullName = Roster.First(player => player.PersonId == teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Ppg[0].PersonId).FullName,
                        StatName = StringConstants.PointsPerGame,
                        StatAvg = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Ppg[0].Value
                    };
                    TeamLeadersPlayers playerApg = new TeamLeadersPlayers
                    {
                        PlayerId = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Apg[0].PersonId,
                        FullName = Roster.First(player => player.PersonId == teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Apg[0].PersonId).FullName,
                        StatName = StringConstants.AssistsPerGame,
                        StatAvg = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Apg[0].Value
                    };
                    TeamLeadersPlayers playerRpg = new TeamLeadersPlayers
                    {
                        PlayerId = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Trpg[0].PersonId,
                        FullName = Roster.First(player => player.PersonId == teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Trpg[0].PersonId).FullName,
                        StatName = StringConstants.ReboundsPerGame,
                        StatAvg = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Trpg[0].Value
                    };
                    TeamLeadersPlayers playerSpg = new TeamLeadersPlayers
                    {
                        PlayerId = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Spg[0].PersonId,
                        FullName = Roster.First(player => player.PersonId == teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Spg[0].PersonId).FullName,
                        StatName = StringConstants.StealsPerGame,
                        StatAvg = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Spg[0].Value
                    };
                    TeamLeadersPlayers playerBpg = new TeamLeadersPlayers
                    {
                        PlayerId = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Bpg[0].PersonId,
                        FullName = Roster.First(player => player.PersonId == teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Bpg[0].PersonId).FullName,
                        StatName = StringConstants.BlocksPerGame,
                        StatAvg = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Bpg[0].Value
                    };
                    TeamLeadersPlayers playerFgp = new TeamLeadersPlayers
                    {
                        PlayerId = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Fgp[0].PersonId,
                        FullName = Roster.First(player => player.PersonId == teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Fgp[0].PersonId).FullName,
                        StatName = StringConstants.FgpPerGame,
                        StatAvg = Math.Round(Convert.ToDecimal(teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Fgp[0].Value) * 100, 1).ToString() + "%"
                    };
                    TeamLeadersPlayers playerFtp = new TeamLeadersPlayers
                    {
                        PlayerId = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Ftp[0].PersonId,
                        FullName = Roster.First(player => player.PersonId == teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Ftp[0].PersonId).FullName,
                        StatName = StringConstants.FtpPerGame,
                        StatAvg = Math.Round(Convert.ToDecimal(teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Ftp[0].Value) * 100, 1).ToString() + "%"
                    };
                    TeamLeadersPlayers playerTpp = new TeamLeadersPlayers
                    {
                        PlayerId = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Tpp[0].PersonId,
                        FullName = Roster.First(player => player.PersonId == teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Tpp[0].PersonId).FullName,
                        StatName = StringConstants.TppPerGame,
                        StatAvg = Math.Round(Convert.ToDecimal(teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Tpp[0].Value) * 100, 1).ToString() + "%"
                    };
                    TeamLeadersPlayers playerTpg = new TeamLeadersPlayers
                    {
                        PlayerId = teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Tpg[0].PersonId,
                        FullName = Roster.First(player => player.PersonId == teamLeaders.LeagueTeamLeaders.NbaTeamLeaders.Tpg[0].PersonId).FullName,
                        StatName = StringConstants.TurnoversPerGame,
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
            catch (NoInternetConnectionException ex)
            {

            }
            
        }

        private async Task GetTeamStats()
        {
            try
            {
                TeamStatsClass teamStat = await NbaApiService.GetTeamStats(_seasonYearApiData);

                if (teamStat != null)
                {
                    TeamStats teamRegularStats = teamStat.LeagueTeamStats.Seasons.RegularSeason.Teams.First(team => team.TeamId == Team.TeamId);

                    teamRegularStats.Fgp.Avg = Math.Round(Convert.ToDecimal(teamRegularStats.Fgp.Avg) * 100, 1).ToString();
                    teamRegularStats.Tpp.Avg = Math.Round(Convert.ToDecimal(teamRegularStats.Tpp.Avg) * 100, 1).ToString();
                    teamRegularStats.Ftp.Avg = Math.Round(Convert.ToDecimal(teamRegularStats.Ftp.Avg) * 100, 1).ToString();

                    TeamRegularStats = teamRegularStats;

                }
            }
            catch (NoInternetConnectionException ex)
            {

            }
            
        }
    }
}

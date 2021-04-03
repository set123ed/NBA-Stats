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
    class StatsViewModel : BaseViewModel
    {
        private string firstTeamAllStarId = "";
        private string lastTeamAllStarId = "";
        public ObservableCollection<Team> TeamList { get; set; } = new ObservableCollection<Team>();
        public ObservableCollection<Player> PlayersList { get; set; } = new ObservableCollection<Player>();

        private ObservableCollection<Team> fullTeamList = new ObservableCollection<Team>();
        private ObservableCollection<Player> fullPlayerList = new ObservableCollection<Player>();

        public bool ShowPlayers { get; set; } = true;
        public bool ShowTeams { get; set; } = false;
        public bool ShowSearch { get; set; } = false;

        public string Filter { get; set; }

        public ObservableCollection<LeadersStatsPlayer> PointsLeaders { get; set; } = new ObservableCollection<LeadersStatsPlayer>(); 
        public ObservableCollection<LeadersStatsPlayer> AssistsLeaders { get; set; } = new ObservableCollection<LeadersStatsPlayer>(); 
        public ObservableCollection<LeadersStatsPlayer> ReboundsLeaders { get; set; } = new ObservableCollection<LeadersStatsPlayer>(); 
        public ObservableCollection<LeadersStatsPlayer> StealsLeaders { get; set; } = new ObservableCollection<LeadersStatsPlayer>(); 
        public ObservableCollection<LeadersStatsPlayer> BlocksLeaders { get; set; } = new ObservableCollection<LeadersStatsPlayer>(); 
        public ObservableCollection<LeadersStatsPlayer> ThreesLeaders { get; set; } = new ObservableCollection<LeadersStatsPlayer>();


        public ObservableCollection<LeadersStatsTeam> TeamPointsLeaders { get; set; } = new ObservableCollection<LeadersStatsTeam>();
        public ObservableCollection<LeadersStatsTeam> TeamAssistsLeaders { get; set; } = new ObservableCollection<LeadersStatsTeam>();
        public ObservableCollection<LeadersStatsTeam> TeamReboundsLeaders { get; set; } = new ObservableCollection<LeadersStatsTeam>();
        public ObservableCollection<LeadersStatsTeam> TeamStealsLeaders { get; set; } = new ObservableCollection<LeadersStatsTeam>();
        public ObservableCollection<LeadersStatsTeam> TeamBlocksLeaders { get; set; } = new ObservableCollection<LeadersStatsTeam>();
        public ObservableCollection<LeadersStatsTeam> TeamThreesLeaders { get; set; } = new ObservableCollection<LeadersStatsTeam>();

        public ICommand SearcherCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand SelectedPlayerCommand { get; }
        public ICommand SelectedTeamCommand { get; }
        public ICommand ShowPlayerCommand { get; }
        public ICommand ShowTeamCommand { get; }
        public ICommand ShowSearchCommand { get; }


        public StatsViewModel(INbaApiService nbaApiServices, INavigationService navigationService) : base(navigationService, nbaApiServices)
        {
            SearcherCommand = new Command(OnSearch);
            ClearCommand = new Command(OnClear);

            SelectedPlayerCommand = new Command<string>(OnSelectedPlayer);
            SelectedTeamCommand = new Command<string>(OnSelectedTeam);

            ShowPlayerCommand = new Command(OnShowPlayer);
            ShowTeamCommand = new Command(OnShowTeam);
            ShowSearchCommand = new Command(OnShowSearch);

            GetData();
        }

        private void OnShowSearch()
        {
            if (!ShowSearch)
            {
                if (ShowPlayers)
                {
                    ShowPlayers = false;
                }
                if (ShowTeams)
                {
                    ShowTeams = false;
                }
                ShowSearch = true;

            }
        }

        private void OnShowTeam()
        {
            if (!ShowTeams)
            {
                if (ShowPlayers)
                {
                    ShowPlayers = false;
                }
                if (ShowSearch)
                {
                    ShowSearch = false;
                    if (!string.IsNullOrEmpty(Filter))
                    {
                        Filter = "";
                    }
                }
                ShowTeams = true;
                
            }
        }

        private void OnShowPlayer()
        {
            if (!ShowPlayers)
            {
                if (ShowTeams)
                {
                    ShowTeams = false;
                }
                if (ShowSearch)
                {
                    ShowSearch = false;

                    if (!string.IsNullOrEmpty(Filter))
                    {
                        Filter = "";
                    }
                    
                }
                ShowPlayers = true;

            }
        }

        private async void OnSelectedTeam(string teamId)
        {
            Team teamSelected = fullTeamList.First(team => team.TeamId == teamId);

            var parameters = new NavigationParameters();
            parameters.Add("team", teamSelected);
            parameters.Add("players", new List<Player>(fullPlayerList));

            await NavigationService.NavigateAsync(Config.TeamProfilePage ,parameters);
        }

        private async void OnSelectedPlayer(string playerId)
        {
            var parameters = new NavigationParameters();
            parameters.Add("teams", new List<Team>(fullTeamList));
            parameters.Add("personId", playerId);
            parameters.Add("players", new List<Player>(fullPlayerList));

            await NavigationService.NavigateAsync(Config.PlayerProfilePage, parameters);
        }

        private void OnClear()
        {
            if (!string.IsNullOrEmpty(Filter))
            {
                Filter = "";
            }
        }

        private void OnSearch()
        {
            if (!string.IsNullOrEmpty(Filter))
            {
                TeamList = new ObservableCollection<Team>(fullTeamList.Where(team => team.FullName.ToLower().Contains(Filter.ToLower())));
                PlayersList = new ObservableCollection<Player>(fullPlayerList.Where(player => (player.FirstName + " " + player.LastName).ToLower().Contains(Filter.ToLower())));
            }
            else 
            {
                TeamList = new ObservableCollection<Team>(fullTeamList);
                PlayersList = new ObservableCollection<Player>(fullPlayerList);
            }
        }

        public async void GetData()
        {
            await GetTeams();
            await GetPlayers();

            await GetPointLeaders();
            await GetAssistLeaders();
            await GetReboundLeaders();
            await GetStealsLeaders();
            await GetBlocksLeaders();
            await GetThreesLeaders();

            await GetTeamStats();
        }

        private async Task GetPlayers()
        {
            var players = await NbaApiService.GetNbaPlayers();

            if (players.GetType().Name == "Players")
            {
                if (players != null)
                {
                    List<Player> playerList = new List<Player>(players.League.Standard);

                    playerList.RemoveAll(player => !player.IsActive);

                    playerList = new List<Player>(playerList.OrderBy(player => player.FirstName));

                    PlayersList = new ObservableCollection<Player>(playerList);
                    fullPlayerList = new ObservableCollection<Player>(playerList);
                }
            }
        }

        public async Task GetTeams()
        {

            var teams = await NbaApiService.GetTeams();

            if (teams.GetType().Name == "Teams")
            {
                if (teams != null)
                {
                    List<Team> teamList = teams.League.Standard;

                    firstTeamAllStarId = teamList.First(team => team.IsAllStar).TeamId;
                    lastTeamAllStarId = teamList.Last(team => team.IsAllStar).TeamId;

                    teamList.RemoveAll(team => team.IsAllStar);
                    TeamList = new ObservableCollection<Team>(teamList);
                    fullTeamList = new ObservableCollection<Team>(teamList);
                }
            }
        }

        public async Task GetPointLeaders()
        {
            var pointsLeaders = await NbaApiService.GetPlayerStatsLeaders("2020-21", "PTS");

            if (pointsLeaders.GetType().Name == "PlayerStatsLeaders")
            {
                if (pointsLeaders != null)
                {
                    ObservableCollection<LeadersStatsPlayer> pointsLeadersList = new ObservableCollection<LeadersStatsPlayer>();
                    for (int i = 0; i < 5; i++)
                    {
                        LeadersStatsPlayer pointLeader = new LeadersStatsPlayer();

                        pointLeader.Pos = i + 1;
                        pointLeader.PlayerId = pointsLeaders.ResultSet.RowSet[i][0].ToString();
                        pointLeader.FullName = pointsLeaders.ResultSet.RowSet[i][2].ToString();
                        pointLeader.Team = pointsLeaders.ResultSet.RowSet[i][3].ToString();

                        string avg = pointsLeaders.ResultSet.RowSet[i][22].ToString();

                        pointLeader.AverageStats = avg.Substring(0, avg.IndexOf('.') + 2);

                        decimal average = Convert.ToDecimal(avg);
                        decimal gamesPlayed = Convert.ToDecimal(pointsLeaders.ResultSet.RowSet[i][4].ToString());

                        pointLeader.TotalStat = Math.Round(average * gamesPlayed,0).ToString();

                        pointsLeadersList.Add(pointLeader);
                    }

                    PointsLeaders = pointsLeadersList;
                }
            }
        }
        public async Task GetAssistLeaders()
        {
            var assistsLeaders = await NbaApiService.GetPlayerStatsLeaders("2020-21", "AST");

            if (assistsLeaders.GetType().Name == "PlayerStatsLeaders")
            {
                if (assistsLeaders != null)
                {
                    ObservableCollection<LeadersStatsPlayer> assistsLeadersList = new ObservableCollection<LeadersStatsPlayer>();
                    for (int i = 0; i < 5; i++)
                    {
                        LeadersStatsPlayer assistLeader = new LeadersStatsPlayer();

                        assistLeader.Pos = i + 1;
                        assistLeader.PlayerId = assistsLeaders.ResultSet.RowSet[i][0].ToString();
                        assistLeader.FullName = assistsLeaders.ResultSet.RowSet[i][2].ToString();
                        assistLeader.Team = assistsLeaders.ResultSet.RowSet[i][3].ToString();

                        string avg = assistsLeaders.ResultSet.RowSet[i][18].ToString();

                        assistLeader.AverageStats = avg.Substring(0,avg.IndexOf('.') + 2);

                        decimal average = Convert.ToDecimal(avg);
                        decimal gamesPlayed = Convert.ToDecimal(assistsLeaders.ResultSet.RowSet[i][4].ToString());

                        assistLeader.TotalStat = Math.Round(average * gamesPlayed,0).ToString();

                        assistsLeadersList.Add(assistLeader);
                    }

                    AssistsLeaders = assistsLeadersList;
                }
            }
        }
        public async Task GetReboundLeaders()
        {
            var reboundsLeaders = await NbaApiService.GetPlayerStatsLeaders("2020-21", "REB");

            if (reboundsLeaders.GetType().Name == "PlayerStatsLeaders")
            {
                if (reboundsLeaders != null)
                {
                    ObservableCollection<LeadersStatsPlayer> reboundsLeadersList = new ObservableCollection<LeadersStatsPlayer>();
                    for (int i = 0; i < 5; i++)
                    {
                        LeadersStatsPlayer reboundLeader = new LeadersStatsPlayer();

                        reboundLeader.Pos = i + 1;
                        reboundLeader.PlayerId = reboundsLeaders.ResultSet.RowSet[i][0].ToString();
                        reboundLeader.FullName = reboundsLeaders.ResultSet.RowSet[i][2].ToString();
                        reboundLeader.Team = reboundsLeaders.ResultSet.RowSet[i][3].ToString();
                        string avg = reboundsLeaders.ResultSet.RowSet[i][17].ToString();

                        reboundLeader.AverageStats = avg.Substring(0, avg.IndexOf('.') + 2);

                        decimal average = Convert.ToDecimal(avg);
                        decimal gamesPlayed = Convert.ToDecimal(reboundsLeaders.ResultSet.RowSet[i][4].ToString());

                        reboundLeader.TotalStat = Math.Round(average * gamesPlayed, 0).ToString();

                        reboundsLeadersList.Add(reboundLeader);
                    }

                    ReboundsLeaders = reboundsLeadersList;
                }
            }
        }

        public async Task GetStealsLeaders()
        {
            var stealsLeaders = await NbaApiService.GetPlayerStatsLeaders("2020-21", "STL");

            if (stealsLeaders.GetType().Name == "PlayerStatsLeaders")
            {
                if (stealsLeaders != null)
                {
                    ObservableCollection<LeadersStatsPlayer> stealsLeadersList = new ObservableCollection<LeadersStatsPlayer>();
                    for (int i = 0; i < 5; i++)
                    {
                        LeadersStatsPlayer stealLeader = new LeadersStatsPlayer();

                        stealLeader.Pos = i + 1;
                        stealLeader.PlayerId = stealsLeaders.ResultSet.RowSet[i][0].ToString();
                        stealLeader.FullName = stealsLeaders.ResultSet.RowSet[i][2].ToString();
                        stealLeader.Team = stealsLeaders.ResultSet.RowSet[i][3].ToString();
                        string avg = stealsLeaders.ResultSet.RowSet[i][19].ToString();

                        stealLeader.AverageStats = avg.Substring(0, avg.IndexOf('.') + 2);

                        decimal average = Convert.ToDecimal(avg);
                        decimal gamesPlayed = Convert.ToDecimal(stealsLeaders.ResultSet.RowSet[i][4].ToString());

                        stealLeader.TotalStat = Math.Round(average * gamesPlayed, 0).ToString();

                        stealsLeadersList.Add(stealLeader);
                    }

                    StealsLeaders = stealsLeadersList;
                }
            }
        }
        public async Task GetBlocksLeaders()
        {
            var blocksLeaders = await NbaApiService.GetPlayerStatsLeaders("2020-21", "BLK");

            if (blocksLeaders.GetType().Name == "PlayerStatsLeaders")
            {
                if (blocksLeaders != null)
                {
                    ObservableCollection<LeadersStatsPlayer> blocksLeadersList = new ObservableCollection<LeadersStatsPlayer>();
                    for (int i = 0; i < 5; i++)
                    {
                        LeadersStatsPlayer blockLeader = new LeadersStatsPlayer();

                        blockLeader.Pos = i + 1;
                        blockLeader.PlayerId = blocksLeaders.ResultSet.RowSet[i][0].ToString();
                        blockLeader.FullName = blocksLeaders.ResultSet.RowSet[i][2].ToString();
                        blockLeader.Team = blocksLeaders.ResultSet.RowSet[i][3].ToString();
                        string avg = blocksLeaders.ResultSet.RowSet[i][20].ToString();

                        blockLeader.AverageStats = avg.Substring(0, avg.IndexOf('.') + 2);

                        decimal average = Convert.ToDecimal(avg);
                        decimal gamesPlayed = Convert.ToDecimal(blocksLeaders.ResultSet.RowSet[i][4].ToString());

                        blockLeader.TotalStat = Math.Round(average * gamesPlayed, 0).ToString();

                        blocksLeadersList.Add(blockLeader);
                    }

                    BlocksLeaders = blocksLeadersList;
                }
            }
        }
        public async Task GetThreesLeaders()
        {
            var threesLeaders = await NbaApiService.GetPlayerStatsLeaders("2020-21", "FG3M");

            if (threesLeaders.GetType().Name == "PlayerStatsLeaders")
            {
                if (threesLeaders != null)
                {
                    ObservableCollection<LeadersStatsPlayer> threesLeadersList = new ObservableCollection<LeadersStatsPlayer>();
                    for (int i = 0; i < 5; i++)
                    {
                        LeadersStatsPlayer threeLeader = new LeadersStatsPlayer();

                        threeLeader.Pos = i + 1;
                        threeLeader.PlayerId = threesLeaders.ResultSet.RowSet[i][0].ToString();
                        threeLeader.FullName = threesLeaders.ResultSet.RowSet[i][2].ToString();
                        threeLeader.Team = threesLeaders.ResultSet.RowSet[i][3].ToString();

                        string avg = threesLeaders.ResultSet.RowSet[i][9].ToString();

                        threeLeader.AverageStats = avg.Substring(0, avg.IndexOf('.') + 2);

                        threeLeader.TotalStat = Math.Round(Convert.ToDecimal(threesLeaders.ResultSet.RowSet[i][11].ToString()) * 100,1).ToString();

                        threesLeadersList.Add(threeLeader);
                    }

                    ThreesLeaders = threesLeadersList;
                }
            }
        }

        public async Task GetTeamStats()
        {
            var teamStats = await NbaApiService.GetTeamStats();

            if (teamStats.GetType().Name == "TeamStatsClass")
            {
                if (teamStats != null)
                {
                    string season = Config.SeasonStages[1].Stage.ToLower();

                    
                    ObservableCollection<TeamStats> statsOfTeams = new ObservableCollection<TeamStats>();
                    ObservableCollection<Series> playoff = new ObservableCollection<Series>();

                    if (season.Contains(" "))
                    {
                        season = season.Remove(season.IndexOf(' '), 1);
                    }
                    else if (season.Contains("-"))
                    {
                        season = season.Remove(season.IndexOf('-'), 1);
                    }


                    if (season == "regularseason" || season == "allstar" || season == "playin")
                    {
                        statsOfTeams = teamStats.LeagueTeamStats.Seasons.RegularSeason.Teams;
                    }
                    else if (season == "preseason")
                    {
                        statsOfTeams = teamStats.LeagueTeamStats.Seasons.Preseason.Teams;
                    }
                    else
                    {
                        playoff = teamStats.LeagueTeamStats.Seasons.Playoffs.Series;

                        List<TeamStats> teamStatsSeries = new List<TeamStats>();

                        foreach (var serie in playoff)
                        {
                            teamStatsSeries.AddRange(serie.Teams);
                        }

                        statsOfTeams = new ObservableCollection<TeamStats>(teamStatsSeries);
                    }

                    TeamStats firstAllStarTeam = new TeamStats();
                    TeamStats lastAllStarTeam = new TeamStats();

                    foreach (TeamStats team in statsOfTeams)
                    {
                        if (team.TeamId == firstTeamAllStarId)
                        {
                            firstAllStarTeam = team;
                        }
                        else if (team.TeamId == lastTeamAllStarId)
                        {
                            lastAllStarTeam = team;
                        }
                    }

                    statsOfTeams.Remove(firstAllStarTeam);
                    statsOfTeams.Remove(lastAllStarTeam);

                    TeamPointsLeaders = GetTeamPpgLeaders(statsOfTeams);
                    TeamAssistsLeaders = GetTeamApgLeaders(statsOfTeams);
                    TeamReboundsLeaders = GetTeamTrpgLeaders(statsOfTeams);
                    TeamStealsLeaders = GetTeamSpgLeaders(statsOfTeams);
                    TeamBlocksLeaders = GetTeamBpgLeaders(statsOfTeams);
                    TeamThreesLeaders = GetTeamTppLeaders(statsOfTeams);

                }
            }
        }

        public ObservableCollection<LeadersStatsTeam> GetTeamPpgLeaders(ObservableCollection<TeamStats> stats)
        {
            ObservableCollection<TeamStats> auxStats = new ObservableCollection<TeamStats>(stats.OrderBy(team => Convert.ToInt32(team.Ppg.Rank)));

            ObservableCollection<LeadersStatsTeam> teamPpgLeader = new ObservableCollection<LeadersStatsTeam>();

            int cont = 0;

            foreach (TeamStats team in auxStats)
            {
                if (cont < 5)
                {
                    LeadersStatsTeam leaderTeam = new LeadersStatsTeam();

                    leaderTeam.Pos = team.Ppg.Rank;
                    leaderTeam.FullName = $"{team.Name} {team.Nickname}";
                    leaderTeam.TeamId = team.TeamId;
                    leaderTeam.AverageStats = team.Ppg.Avg;

                    teamPpgLeader.Add(leaderTeam);

                    cont++;
                }
                else
                {
                    break;
                }
            }

            return teamPpgLeader;
        }

        public ObservableCollection<LeadersStatsTeam> GetTeamApgLeaders(ObservableCollection<TeamStats> stats)
        {
            ObservableCollection<TeamStats> auxStats = new ObservableCollection<TeamStats>(stats.OrderBy(team => Convert.ToInt32(team.Apg.Rank)));

            ObservableCollection<LeadersStatsTeam> teamApgLeader = new ObservableCollection<LeadersStatsTeam>();

            int cont = 0;

            foreach (TeamStats team in auxStats)
            {
                if (cont < 5)
                {
                    LeadersStatsTeam leaderTeam = new LeadersStatsTeam();

                    leaderTeam.Pos = team.Apg.Rank;
                    leaderTeam.FullName = $"{team.Name} {team.Nickname}";
                    leaderTeam.TeamId = team.TeamId;
                    leaderTeam.AverageStats = team.Apg.Avg;

                    teamApgLeader.Add(leaderTeam);
                    cont++;
                }
                else
                {
                    break;
                }
            }

            return teamApgLeader;
        }

        public ObservableCollection<LeadersStatsTeam> GetTeamTrpgLeaders(ObservableCollection<TeamStats> stats)
        {
            ObservableCollection<TeamStats> auxStats = new ObservableCollection<TeamStats>(stats.OrderBy(team => Convert.ToInt32(team.Trpg.Rank)));

            ObservableCollection<LeadersStatsTeam> teamTrpgLeader = new ObservableCollection<LeadersStatsTeam>();

            int cont = 0;

            foreach (TeamStats team in auxStats)
            {
                if (cont < 5)
                {
                    LeadersStatsTeam leaderTeam = new LeadersStatsTeam();

                    leaderTeam.Pos = team.Trpg.Rank;
                    leaderTeam.FullName = $"{team.Name} {team.Nickname}";
                    leaderTeam.TeamId = team.TeamId;
                    leaderTeam.AverageStats = team.Trpg.Avg;

                    teamTrpgLeader.Add(leaderTeam);
                    cont++;
                }
                else
                {
                    break;
                }
            }

            return teamTrpgLeader;
        }

        public ObservableCollection<LeadersStatsTeam> GetTeamSpgLeaders(ObservableCollection<TeamStats> stats)
        {
            ObservableCollection<TeamStats> auxStats = new ObservableCollection<TeamStats>(stats.OrderBy(team => Convert.ToInt32(team.Spg.Rank)));

            ObservableCollection<LeadersStatsTeam> teamSpgLeader = new ObservableCollection<LeadersStatsTeam>();

            int cont = 0;

            foreach (TeamStats team in auxStats)
            {
                if (cont < 5)
                {
                    LeadersStatsTeam leaderTeam = new LeadersStatsTeam();

                    leaderTeam.Pos = team.Spg.Rank;
                    leaderTeam.FullName = $"{team.Name} {team.Nickname}";
                    leaderTeam.TeamId = team.TeamId;
                    leaderTeam.AverageStats = team.Spg.Avg;

                    teamSpgLeader.Add(leaderTeam);
                    cont++;
                }
                else
                {
                    break;
                }
            }

            return teamSpgLeader;
        }

        public ObservableCollection<LeadersStatsTeam> GetTeamBpgLeaders(ObservableCollection<TeamStats> stats)
        {
            ObservableCollection<TeamStats> auxStats = new ObservableCollection<TeamStats>(stats.OrderBy(team => Convert.ToInt32(team.Bpg.Rank)));

            ObservableCollection<LeadersStatsTeam> teamBpgLeader = new ObservableCollection<LeadersStatsTeam>();

            int cont = 0;

            foreach (TeamStats team in auxStats)
            {
                if (cont < 5)
                {
                    LeadersStatsTeam leaderTeam = new LeadersStatsTeam();

                    leaderTeam.Pos = team.Bpg.Rank;
                    leaderTeam.FullName = $"{team.Name} {team.Nickname}";
                    leaderTeam.TeamId = team.TeamId;
                    leaderTeam.AverageStats = team.Bpg.Avg;

                    teamBpgLeader.Add(leaderTeam);
                    cont++;
                }
                else
                {
                    break;
                }
            }

            return teamBpgLeader;
        }

        public ObservableCollection<LeadersStatsTeam> GetTeamTppLeaders(ObservableCollection<TeamStats> stats)
        {
            ObservableCollection<TeamStats> auxStats = new ObservableCollection<TeamStats>(stats.OrderBy(team => Convert.ToInt32(team.Tpp.Rank)));

            ObservableCollection<LeadersStatsTeam> teamTppLeader = new ObservableCollection<LeadersStatsTeam>();

            int cont = 0;

            foreach (TeamStats team in auxStats)
            {
                if (cont < 5)
                {
                    LeadersStatsTeam leaderTeam = new LeadersStatsTeam();

                    leaderTeam.Pos = team.Tpp.Rank;
                    leaderTeam.FullName = $"{team.Name} {team.Nickname}";
                    leaderTeam.TeamId = team.TeamId;
                    leaderTeam.AverageStats = Math.Round(Convert.ToDecimal(team.Tpp.Avg) * 100,0).ToString();

                    teamTppLeader.Add(leaderTeam);
                    cont++;
                }
                else
                {
                    break;
                }
            }

            return teamTppLeader;
        }
    }
}

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
    public class StatsViewModel : BaseViewModel
    {
        public ObservableCollection<Team> TeamList { get; set; } = new ObservableCollection<Team>();
        public ObservableCollection<Player> PlayersList { get; set; } = new ObservableCollection<Player>();

        public bool ShowPlayers { get; set; } = true;
        public bool ShowTeams { get; set; } = false;
        public bool ShowSearch { get; set; } = false;

        public string Filter { get; set; }
        public ObservableCollection<LeaderStatsPlayerCollection> LeaderStatsPlayers { get; set; } = new ObservableCollection<LeaderStatsPlayerCollection>();
        public ObservableCollection<LeaderStatsTeamCollection> LeadersStatsTeams { get; set; } = new ObservableCollection<LeaderStatsTeamCollection>();
        public ICommand SearcherCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand SelectedPlayerCommand { get; }
        public ICommand SelectedTeamCommand { get; }
        public ICommand ShowPlayerCommand { get; }
        public ICommand ShowTeamCommand { get; }
        public ICommand ShowSearchCommand { get; }

        public bool IsBusy { get; set; } = true;
        public bool IsNotBusy => !IsBusy;

        public StatsViewModel(INbaApiService nbaApiService, INavigationService navigationService, INbaDefaultInfoService nbaDefaultInfoService,IDatabaseService baseServices) : base(navigationService, nbaApiService, nbaDefaultInfoService,baseServices)
        {
            SearcherCommand = new Command(OnSearch);
            ClearCommand = new Command(OnClear);

            SelectedPlayerCommand = new Command<string>(OnSelectedPlayer);
            SelectedTeamCommand = new Command<string>(OnSelectedTeam);

            ShowPlayerCommand = new Command(OnShowPlayer);
            ShowTeamCommand = new Command(OnShowTeam);
            ShowSearchCommand = new Command(OnShowSearch);

            GetStatsData();
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
            Team teamSelected = _teamList.First(team => team.TeamId == teamId);

            var parameters = new NavigationParameters();
            parameters.Add(ParametersConstants.Team, teamSelected);

            await NavigationService.NavigateAsync(NavigationConstants.TeamProfilePage ,parameters);
        }

        private async void OnSelectedPlayer(string playerId)
        {
            var parameters = new NavigationParameters();
            parameters.Add(ParametersConstants.PlayerId, playerId);

            await NavigationService.NavigateAsync(NavigationConstants.PlayerProfilePage, parameters);
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
                TeamList = new ObservableCollection<Team>(_teamList.Where(team => team.FullName.ToLower().Contains(Filter.ToLower())));
                PlayersList = new ObservableCollection<Player>(_playerList.Where(player => (player.FullName).ToLower().Contains(Filter.ToLower())));
            }
            else 
            {
                TeamList = new ObservableCollection<Team>(_teamList);
                PlayersList = new ObservableCollection<Player>(_playerList);
            }
        }

        public async void GetStatsData()
        {
            await GetDefaultData();

            await GetPointLeaders();
            await GetAssistLeaders();
            await GetReboundLeaders();
            await GetStealsLeaders();
            await GetBlocksLeaders();
            await GetThreesLeaders();

            await GetTeamStats();

            PlayersList = new ObservableCollection<Player>(_playerList);
            TeamList = new ObservableCollection<Team>(_teamList);
            IsBusy = false;
        }

        public async Task GetPointLeaders()
        {
            try
            {
                PlayerStatsLeaders pointsLeaders = await NbaApiService.GetPlayerStatsLeaders(_seasonApiStats, StringConstants.PtsStatParameter);

                if (pointsLeaders != null)
                {
                    LeaderStatsPlayerCollection pointsLeadersList = new LeaderStatsPlayerCollection(StringConstants.PointsPerGame);
                    for (int i = 0; i < 5; i++)
                    {
                        LeadersStatsPlayer pointLeader = new LeadersStatsPlayer();

                        pointLeader.Pos = i + 1;
                        pointLeader.PlayerId = pointsLeaders.ResultSet.RowSet[i][0].ToString();
                        pointLeader.FullName = pointsLeaders.ResultSet.RowSet[i][2].ToString();
                        pointLeader.Team = pointsLeaders.ResultSet.RowSet[i][3].ToString();
                        pointLeader.TeamId = _teamList.First(team => team.Tricode.ToLower() == pointLeader.Team.ToString().ToLower()).TeamId;
                        string avg = pointsLeaders.ResultSet.RowSet[i][22].ToString();

                        pointLeader.AverageStats = avg.Substring(0, avg.IndexOf('.') + 2);

                        decimal average = Convert.ToDecimal(avg);
                        decimal gamesPlayed = Convert.ToDecimal(pointsLeaders.ResultSet.RowSet[i][4].ToString());

                        pointLeader.TotalStat = Math.Round(average * gamesPlayed, 0).ToString();

                        pointsLeadersList.Add(pointLeader);
                    }
                    LeaderStatsPlayers.Add(pointsLeadersList);
                }
            }
            catch (NoInternetConnectionException ex)
            {

            }
            
        }
        public async Task GetAssistLeaders()
        {
            try
            {
                PlayerStatsLeaders assistsLeaders = await NbaApiService.GetPlayerStatsLeaders(_seasonApiStats, StringConstants.AstStatParameter);

                if (assistsLeaders != null)
                {
                    LeaderStatsPlayerCollection assistsLeadersList = new LeaderStatsPlayerCollection(StringConstants.AssistsPerGame);
                    for (int i = 0; i < 5; i++)
                    {
                        LeadersStatsPlayer assistLeader = new LeadersStatsPlayer();

                        assistLeader.Pos = i + 1;
                        assistLeader.PlayerId = assistsLeaders.ResultSet.RowSet[i][0].ToString();
                        assistLeader.FullName = assistsLeaders.ResultSet.RowSet[i][2].ToString();
                        assistLeader.Team = assistsLeaders.ResultSet.RowSet[i][3].ToString();
                        assistLeader.TeamId = _teamList.First(team => team.Tricode.ToLower() == assistLeader.Team.ToString().ToLower()).TeamId;

                        string avg = assistsLeaders.ResultSet.RowSet[i][18].ToString();

                        assistLeader.AverageStats = avg.Substring(0, avg.IndexOf('.') + 2);

                        decimal average = Convert.ToDecimal(avg);
                        decimal gamesPlayed = Convert.ToDecimal(assistsLeaders.ResultSet.RowSet[i][4].ToString());

                        assistLeader.TotalStat = Math.Round(average * gamesPlayed, 0).ToString();

                        assistsLeadersList.Add(assistLeader);
                    }

                    LeaderStatsPlayers.Add(assistsLeadersList);
                }
            }
            catch (NoInternetConnectionException ex)
            {

            }

            
        }
        public async Task GetReboundLeaders()
        {
            try
            {
                PlayerStatsLeaders reboundsLeaders = await NbaApiService.GetPlayerStatsLeaders(_seasonApiStats, StringConstants.RebStatParameter);

                if (reboundsLeaders != null)
                {
                    LeaderStatsPlayerCollection reboundsLeadersList = new LeaderStatsPlayerCollection(StringConstants.ReboundsPerGame);
                    for (int i = 0; i < 5; i++)
                    {
                        LeadersStatsPlayer reboundLeader = new LeadersStatsPlayer();

                        reboundLeader.Pos = i + 1;
                        reboundLeader.PlayerId = reboundsLeaders.ResultSet.RowSet[i][0].ToString();
                        reboundLeader.FullName = reboundsLeaders.ResultSet.RowSet[i][2].ToString();
                        reboundLeader.Team = reboundsLeaders.ResultSet.RowSet[i][3].ToString();
                        reboundLeader.TeamId = _teamList.First(team => team.Tricode.ToLower() == reboundLeader.Team.ToString().ToLower()).TeamId;

                        string avg = reboundsLeaders.ResultSet.RowSet[i][17].ToString();

                        reboundLeader.AverageStats = avg.Substring(0, avg.IndexOf('.') + 2);

                        decimal average = Convert.ToDecimal(avg);
                        decimal gamesPlayed = Convert.ToDecimal(reboundsLeaders.ResultSet.RowSet[i][4].ToString());

                        reboundLeader.TotalStat = Math.Round(average * gamesPlayed, 0).ToString();

                        reboundsLeadersList.Add(reboundLeader);
                    }

                    LeaderStatsPlayers.Add(reboundsLeadersList);
                }


            }
            catch (NoInternetConnectionException ex)
            {

            }
        }

        public async Task GetStealsLeaders()
        {
            try
            {
                PlayerStatsLeaders stealsLeaders = await NbaApiService.GetPlayerStatsLeaders(_seasonApiStats, StringConstants.StlStatParameter);

                if (stealsLeaders != null)
                {
                    LeaderStatsPlayerCollection stealsLeadersList = new LeaderStatsPlayerCollection(StringConstants.StealsPerGame);
                    for (int i = 0; i < 5; i++)
                    {
                        LeadersStatsPlayer stealLeader = new LeadersStatsPlayer();

                        stealLeader.Pos = i + 1;
                        stealLeader.PlayerId = stealsLeaders.ResultSet.RowSet[i][0].ToString();
                        stealLeader.FullName = stealsLeaders.ResultSet.RowSet[i][2].ToString();
                        stealLeader.Team = stealsLeaders.ResultSet.RowSet[i][3].ToString();
                        stealLeader.TeamId = _teamList.First(team => team.Tricode.ToLower() == stealLeader.Team.ToString().ToLower()).TeamId;

                        string avg = stealsLeaders.ResultSet.RowSet[i][19].ToString();

                        stealLeader.AverageStats = avg.Substring(0, avg.IndexOf('.') + 2);

                        decimal average = Convert.ToDecimal(avg);
                        decimal gamesPlayed = Convert.ToDecimal(stealsLeaders.ResultSet.RowSet[i][4].ToString());

                        stealLeader.TotalStat = Math.Round(average * gamesPlayed, 0).ToString();

                        stealsLeadersList.Add(stealLeader);
                    }

                    LeaderStatsPlayers.Add(stealsLeadersList);
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public async Task GetBlocksLeaders()
        {
            try
            {
                PlayerStatsLeaders blocksLeaders = await NbaApiService.GetPlayerStatsLeaders(_seasonApiStats, StringConstants.BlkStatParameter);

                if (blocksLeaders != null)
                {
                    LeaderStatsPlayerCollection blocksLeadersList = new LeaderStatsPlayerCollection(StringConstants.BlocksPerGame);
                    for (int i = 0; i < 5; i++)
                    {
                        LeadersStatsPlayer blockLeader = new LeadersStatsPlayer();

                        blockLeader.Pos = i + 1;
                        blockLeader.PlayerId = blocksLeaders.ResultSet.RowSet[i][0].ToString();
                        blockLeader.FullName = blocksLeaders.ResultSet.RowSet[i][2].ToString();
                        blockLeader.Team = blocksLeaders.ResultSet.RowSet[i][3].ToString();
                        blockLeader.TeamId = _teamList.First(team => team.Tricode.ToLower() == blockLeader.Team.ToString().ToLower()).TeamId;

                        string avg = blocksLeaders.ResultSet.RowSet[i][20].ToString();

                        blockLeader.AverageStats = avg.Substring(0, avg.IndexOf('.') + 2);

                        decimal average = Convert.ToDecimal(avg);
                        decimal gamesPlayed = Convert.ToDecimal(blocksLeaders.ResultSet.RowSet[i][4].ToString());

                        blockLeader.TotalStat = Math.Round(average * gamesPlayed, 0).ToString();

                        blocksLeadersList.Add(blockLeader);
                    }
                    LeaderStatsPlayers.Add(blocksLeadersList);
                }
            }
            catch (NoInternetConnectionException ex)
            {

            }
            
        }
        public async Task GetThreesLeaders()
        {
            try
            {
                PlayerStatsLeaders threesLeaders = await NbaApiService.GetPlayerStatsLeaders(_seasonApiStats, StringConstants.Fg3mStatParameter);

                if (threesLeaders != null)
                {
                    LeaderStatsPlayerCollection threesLeadersList = new LeaderStatsPlayerCollection(StringConstants.TpmgPerGame);
                    for (int i = 0; i < 5; i++)
                    {
                        LeadersStatsPlayer threeLeader = new LeadersStatsPlayer();

                        threeLeader.Pos = i + 1;
                        threeLeader.PlayerId = threesLeaders.ResultSet.RowSet[i][0].ToString();
                        threeLeader.FullName = threesLeaders.ResultSet.RowSet[i][2].ToString();
                        threeLeader.Team = threesLeaders.ResultSet.RowSet[i][3].ToString();
                        threeLeader.TeamId = _teamList.First(team => team.Tricode.ToLower() == threeLeader.Team.ToString().ToLower()).TeamId;

                        string avg = threesLeaders.ResultSet.RowSet[i][9].ToString();

                        threeLeader.AverageStats = avg.Substring(0, avg.IndexOf('.') + 2);

                        threeLeader.TotalStat = Math.Round(Convert.ToDecimal(threesLeaders.ResultSet.RowSet[i][11].ToString()) * 100, 1).ToString();

                        threesLeadersList.Add(threeLeader);
                    }

                    LeaderStatsPlayers.Add(threesLeadersList);
                }
            }
            catch (NoInternetConnectionException ex)
            {

            }

            
        }

        public async Task GetTeamStats()
        {
            try
            {
                TeamStatsClass teamStats = await NbaApiService.GetTeamStats(_seasonYearApiData);


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


                    if (season == StringConstants.RegularSeasonFilter || season == StringConstants.AllStarSeasonFilter || season == StringConstants.PlayInSeasonFilter)
                    {
                        statsOfTeams = teamStats.LeagueTeamStats.Seasons.RegularSeason.Teams;
                    }
                    else if (season == StringConstants.PreSeasonFilter)
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

                    string firstTeamAllStarId = _teamList.First(team => team.IsAllStar).TeamId;
                    string lastTeamAllStarId = _teamList.Last(team => team.IsAllStar).TeamId;

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

                    LeadersStatsTeams.Add(GetTeamPpgLeaders(statsOfTeams));
                    LeadersStatsTeams.Add(GetTeamApgLeaders(statsOfTeams));
                    LeadersStatsTeams.Add(GetTeamTrpgLeaders(statsOfTeams));
                    LeadersStatsTeams.Add(GetTeamSpgLeaders(statsOfTeams));
                    LeadersStatsTeams.Add(GetTeamBpgLeaders(statsOfTeams));
                    LeadersStatsTeams.Add(GetTeamTppLeaders(statsOfTeams));

                }

            }
            catch (NoInternetConnectionException ex)
            {

            }
            
        }

        public LeaderStatsTeamCollection GetTeamPpgLeaders(ObservableCollection<TeamStats> stats)
        {
            ObservableCollection<TeamStats> auxStats = new ObservableCollection<TeamStats>(stats.OrderBy(team => Convert.ToInt32(team.Ppg.Rank)));

            LeaderStatsTeamCollection teamPpgLeader = new LeaderStatsTeamCollection(StringConstants.PointsPerGame);

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

        public LeaderStatsTeamCollection GetTeamApgLeaders(ObservableCollection<TeamStats> stats)
        {
            ObservableCollection<TeamStats> auxStats = new ObservableCollection<TeamStats>(stats.OrderBy(team => Convert.ToInt32(team.Apg.Rank)));

            LeaderStatsTeamCollection teamApgLeader = new LeaderStatsTeamCollection(StringConstants.AssistsPerGame);

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

        public LeaderStatsTeamCollection GetTeamTrpgLeaders(ObservableCollection<TeamStats> stats)
        {
            ObservableCollection<TeamStats> auxStats = new ObservableCollection<TeamStats>(stats.OrderBy(team => Convert.ToInt32(team.Trpg.Rank)));

            LeaderStatsTeamCollection teamTrpgLeader = new LeaderStatsTeamCollection(StringConstants.ReboundsPerGame);

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

        public LeaderStatsTeamCollection GetTeamSpgLeaders(ObservableCollection<TeamStats> stats)
        {
            ObservableCollection<TeamStats> auxStats = new ObservableCollection<TeamStats>(stats.OrderBy(team => Convert.ToInt32(team.Spg.Rank)));

            LeaderStatsTeamCollection teamSpgLeader = new LeaderStatsTeamCollection(StringConstants.StealsPerGame);

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

        public LeaderStatsTeamCollection GetTeamBpgLeaders(ObservableCollection<TeamStats> stats)
        {
            ObservableCollection<TeamStats> auxStats = new ObservableCollection<TeamStats>(stats.OrderBy(team => Convert.ToInt32(team.Bpg.Rank)));

            LeaderStatsTeamCollection teamBpgLeader = new LeaderStatsTeamCollection(StringConstants.BlocksPerGame);

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

        public LeaderStatsTeamCollection GetTeamTppLeaders(ObservableCollection<TeamStats> stats)
        {
            ObservableCollection<TeamStats> auxStats = new ObservableCollection<TeamStats>(stats.OrderBy(team => Convert.ToInt32(team.Tpp.Rank)));

            LeaderStatsTeamCollection teamTppLeader = new LeaderStatsTeamCollection(StringConstants.TppPerGame);

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

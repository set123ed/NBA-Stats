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

namespace NBAStats.ViewModels
{
    public class PlayerProfileViewModel : BaseViewModel, IInitialize
    {

        private List<Team> teamList = new List<Team>();
        private List<Player> playersList = new List<Player>();

        public Team ActualTeam { get; set; }
        public Player PlayerInfo { get; set; }

        public string PlayerHeight { get; set; }
        public string ActualTeamInfo { get; set; }
        public string YearDebutActualTeam { get; set; }
        public ObservableCollection<StatsPerSeasonCollection> Seasons { get; set; } = new ObservableCollection<StatsPerSeasonCollection>();

        private List<ActivePlayerBoxScore> activePlayersList = new List<ActivePlayerBoxScore>();
        private string playerId = "";
        public PlayerStandard PlayerStats { get; set; }
        public StatsPlayerProfile CarrerSumarry { get; set; }
        public StatsPlayerProfile ActualSeason { get; set; }
        public PlayerProfileViewModel(INbaApiService nbaApiService, INavigationService navigationService) : base(navigationService, nbaApiService)
        {

        }

        public async void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(ParametersConstants.TeamList, out List<Team> teams) && parameters.TryGetValue(ParametersConstants.PlayerId, out string personId) &&
                parameters.TryGetValue(ParametersConstants.PlayerList, out List<Player> players))
            {
                teamList = teams;
                playerId = personId;
                playersList = players;

                if (!string.IsNullOrEmpty(playerId))
                {
                    await GetProfile(playerId);
                }
            }
        }

        public async Task GetProfile(string personId)
        {
            var playerProfile = await NbaApiService.GetPlayerProfile(personId);

            if (playerProfile.GetType().Name == "PlayerProfile")
            {
                if (playerProfile != null)
                {
                    PlayerStats = playerProfile.League.Standard;


                    foreach (SeasonPlayerProfile season in PlayerStats.Stats.RegularSeason.Season)
                    {
                        string seasonYear = season.SeasonYear.ToString();
                        StatsPerSeasonCollection seasonList = new StatsPerSeasonCollection(seasonYear);
                        foreach (StatsPlayerProfile teams in season.Teams)
                        {
                            foreach (Team team in teamList)
                            {
                                if (teams.TeamId == team.TeamId)
                                {
                                    teams.TriCodeTeam = team.Tricode;
                                    seasonList.Add(teams);
                                }
                            }


                        }

                        Seasons.Add(seasonList);
                    }

                    decimal turnovers = Convert.ToDecimal(PlayerStats.Stats.CareerSummary.Turnovers),
                            gamesPlayed = Convert.ToDecimal(PlayerStats.Stats.CareerSummary.GamesPlayed),
                            topg = Math.Round(turnovers / gamesPlayed, 2);

                    PlayerStats.Stats.CareerSummary.Topg = topg.ToString();
                    CarrerSumarry = PlayerStats.Stats.CareerSummary;

                    ActualSeason = PlayerStats.Stats.Latest;

                    ActualTeam = teamList.First(team => team.TeamId == PlayerStats.TeamId);

                    Player playerInfo = playersList.First(player => player.PersonId == personId);
                    playerInfo.FullName = $"{playerInfo.FirstName} {playerInfo.LastName}";

                    PlayerHeight = $"{playerInfo.HeightFeet}.{playerInfo.HeightInches}";

                    ActualTeamInfo = $"In {ActualTeam.Tricode} since: ";
                    YearDebutActualTeam = playerInfo.Teams[playerInfo.Teams.Count - 1].SeasonStart;

                    PlayerInfo = playerInfo;

                }
            }
        }
    }
}

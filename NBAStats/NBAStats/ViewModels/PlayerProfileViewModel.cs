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

        private List<Team> _teamList = new List<Team>();
        private List<Player> _playersList = new List<Player>();

        public Team ActualTeam { get; set; }
        public Player PlayerInfo { get; set; }
        public string ActualTeamInfo { get; set; }

        public ObservableCollection<StatsPerSeasonCollection> Seasons { get; set; } = new ObservableCollection<StatsPerSeasonCollection>();

        private string _playerId = "";
        public PlayerStandard PlayerStats { get; set; }
        public StatsPlayerProfile CarrerSumarry { get; set; }
        public StatsPlayerProfile ActualSeason { get; set; }

        public bool IsBusy { get; set; } = true;
        public bool IsNotBusy => !IsBusy;
        public PlayerProfileViewModel(INbaApiService nbaApiService, INavigationService navigationService) : base(navigationService, nbaApiService)
        {

        }

        public async void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(ParametersConstants.TeamList, out List<Team> teamsList) && parameters.TryGetValue(ParametersConstants.PlayerId, out string personId) &&
                parameters.TryGetValue(ParametersConstants.PlayerList, out List<Player> playersList))
            {
                _teamList = teamsList;
                _playerId = personId;
                _playersList = playersList;

                if (!string.IsNullOrEmpty(_playerId))
                {
                    await GetSeasonYearParameters();
                    await GetProfile(_playerId);
                    IsBusy = false;
                }
            }
        }

        public async Task GetProfile(string personId)
        {
            var playerProfile = await NbaApiService.GetPlayerProfile(_seasonYearApiData, personId);

            if (playerProfile.GetType().Name == "PlayerProfile")
            {
                if (playerProfile != null)
                {
                    PlayerStats = playerProfile.League.Standard;


                    foreach (SeasonPlayerProfile season in PlayerStats.Stats.RegularSeason.Season)
                    {
                        string seasonYear = $"{season.SeasonYear}-{season.SeasonYear + 1}";
                        StatsPerSeasonCollection seasonList = new StatsPerSeasonCollection(seasonYear);
                        foreach (StatsPlayerProfile teams in season.Teams)
                        {
                            foreach (Team team in _teamList)
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
                            topg = Math.Round(turnovers / gamesPlayed, 1);

                    PlayerStats.Stats.CareerSummary.Topg = topg.ToString();
                    CarrerSumarry = PlayerStats.Stats.CareerSummary;

                    ActualSeason = PlayerStats.Stats.Latest;

                    ActualTeam = _teamList.First(team => team.TeamId == PlayerStats.TeamId);

                    Player playerInfo = _playersList.First(player => player.PersonId == personId);

                    ActualTeamInfo = $"In {ActualTeam.Tricode} since: ";


                    PlayerInfo = playerInfo;

                }
            }
        }
    }
}

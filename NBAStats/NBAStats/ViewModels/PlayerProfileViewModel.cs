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
        public PlayerProfileViewModel(INbaApiService nbaApiService, INavigationService navigationService, INbaDefaultInfoService nbaDefaultInfoService) : base(navigationService, nbaApiService, nbaDefaultInfoService)
        {

        }

        public async void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(ParametersConstants.PlayerId, out string personId))
            {
                _playerId = personId;

                if (!string.IsNullOrEmpty(_playerId))
                {
                    await GetDefaultData();
                    await GetProfile(_playerId);
                    IsBusy = false;
                }
            }
        }

        public async Task GetProfile(string personId)
        {
            try
            {
                PlayerProfile playerProfile = await NbaApiService.GetPlayerProfile(_seasonYearApiData, personId);

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

                    Player playerInfo = _playerList.First(player => player.PersonId == personId);

                    ActualTeamInfo = $"In {ActualTeam.Tricode} since: ";


                    PlayerInfo = playerInfo;

                }

            }
            catch (NoInternetConnectionException ex)
            {

            }
            
        }
    }
}

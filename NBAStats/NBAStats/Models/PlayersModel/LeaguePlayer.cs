using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json.Serialization;

namespace NBAStats.Models.PlayersModel
{
    public class LeaguePlayer
    {

        [JsonPropertyName("standard")]
        public ObservableCollection<Player> Standard { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

namespace NBAStats.Models.PlayersModel
{
    public class PlayerList : INotifyPropertyChanged
    {
        [JsonPropertyName("league")]
        public LeaguePlayer League { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

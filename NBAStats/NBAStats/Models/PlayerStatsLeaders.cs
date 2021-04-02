﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

namespace NBAStats.Models
{

    public class ResultSet : INotifyPropertyChanged
    {

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("headers")]
        public IList<string> Headers { get; set; }

        [JsonPropertyName("rowSet")]
        public IList<IList<object>> RowSet { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class PlayerStatsLeaders : INotifyPropertyChanged
    {

        [JsonPropertyName("resource")]
        public string Resource { get; set; }

        [JsonPropertyName("resultSet")]
        public ResultSet ResultSet { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class PlayerRegularStats :INotifyPropertyChanged
    {
        public string PlayerId { get; set; }

        public string Name { get; set; }
        public string Team { get; set; }
        public string PointsPerGame { get; set; }
        public string AssistsPerGame { get; set; }
        public string ReboundsPerGame { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class LeadersStatsPlayer
    {
        public int Pos { get; set; }
        public string PlayerId { get; set; }
        public string FullName { get; set; }
        public string Team { get; set; }
        public string AverageStats { get; set; }
        public string TotalStat { get; set; }
    }
}

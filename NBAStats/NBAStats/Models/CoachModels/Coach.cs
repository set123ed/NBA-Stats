using System;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;

namespace NBAStats.Models.CoachModels
{
    public class Coach
    {

        public class CoachInfo : INotifyPropertyChanged
        {
            [JsonPropertyName("league")]
            public League League { get; set; }
            public event PropertyChangedEventHandler PropertyChanged;

            public static implicit operator ObservableCollection<object>(CoachInfo v)
            {
                throw new NotImplementedException();
            }
        }
    }
}


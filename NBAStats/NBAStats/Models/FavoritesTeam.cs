using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats.Models
{
    public class FavoritesTeam
    {
        [PrimaryKey]
        public string TeamId { get; set; }
        public string Name { get; set; }
        public string Tricode { get; set; }
        public string TeamLogo { get; set; }
    }
}

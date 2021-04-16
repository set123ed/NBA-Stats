using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats.Models
{
    public class FavoritesPlayer
    {
        [PrimaryKey]
        public string PlayerId { get; set; }

        public string Name { get; set; }
        public string TeamId { get; set; }
        public string TeamLogo { get; set; }
    }
}

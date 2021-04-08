using System;
using System.Collections.Generic;
using System.Text;
using SQLite;


namespace NBAStats.Models
{
    public class FavoriteTeam
    {
        [MaxLength(50)]
        public string IdFavoriteTeam { get; set; }
        [MaxLength(40)]
        public string Name { get; set; }
    }
}

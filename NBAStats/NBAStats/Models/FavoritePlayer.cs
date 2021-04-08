using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using SQLite;

namespace NBAStats.Models
{
    public class FavoritePlayer
    {
        [MaxLength(100)]
        public string IdFavoritePlayer { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
    }
}

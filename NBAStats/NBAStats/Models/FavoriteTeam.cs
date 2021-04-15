using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace NBAStats.Models
{
    public class FavoriteTeam
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        [MaxLength(100)]
        public string IdFavoriteTeam { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats.Models
{
    public class SeasonStage
    {
        public SeasonStage(int id, string stage)
        {
            Id = id;
            Stage = stage;
        }

        public int Id { get; }
        public string Stage { get; }

    }
}

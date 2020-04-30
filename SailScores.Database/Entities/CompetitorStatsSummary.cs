﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SailScores.Database.Entities
{
    public class CompetitorStatsSummary
    {
        public Guid CompetitorId { get; set; }
        public string SeasonName { get; set; }
        public DateTime SeasonStart { get; set; }
        public DateTime SeasonEnd { get; set; }
        public int RaceCount { get; set; }
        public double? AverageFinishRank { get; set; }
        public int? DaysRaced { get; set; }
        public int? RaceFinishers { get; set; }
        public int? BoatsRacedAgainst { get; set; }
        public int? BoatsBeat { get; set; }
    }
}
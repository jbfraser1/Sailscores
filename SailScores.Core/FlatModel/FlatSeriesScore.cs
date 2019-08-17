﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SailScores.Core.FlatModel
{
    public class FlatSeriesScore
    {
        public Guid CompetitorId { get; set; }
        public int? Rank { get; set; }
        public int? Trend { get; set; }
        public Decimal? TotalScore { get; set; }
        public Decimal? PointsEarned { get; set; }
        public Decimal? PointsPossible { get; set; }
        public IEnumerable<FlatCalculatedScore> Scores { get; set; }
    }
}
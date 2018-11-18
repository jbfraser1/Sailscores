﻿using System.Collections.Generic;
using System.Linq;
using Sailscores.ImportExport.Sailwave.Elements;
using Sailscores.ImportExport.Sailwave.Elements.File;

namespace Sailscores.ImportExport.Sailwave.Parsers
{
    public class CompetitorSetParser
    {
        public static List<Competitor> GetCompetitors(IEnumerable<FileRow> rows)
        {
            var returnList = new List<Competitor>();
            List<int?> competitorIds = rows
                .Where(r => r.RowType == RowType.Competitor)
                .Select(c => c.CompetitorOrScoringSystemId)
                .Distinct()
                .ToList();
            var parser = new CompetitorParser();
            foreach (var competitorId in competitorIds)
            {
                var comp = parser.LoadType(
                    rows.Where(r => r.RowType == RowType.Competitor
                        && r.CompetitorOrScoringSystemId == competitorId));

                returnList.Add(comp);
            }

            return returnList;
        }
    }
}

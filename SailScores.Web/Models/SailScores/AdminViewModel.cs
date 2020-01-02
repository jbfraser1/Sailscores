﻿using SailScores.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SailScores.Web.Models.SailScores
{
    // Within the context of a single club, list things that can be administered.
    public class AdminViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public String Name { get; set; }

        [StringLength(10)]
        public String Initials { get; set; }
        public String Description { get; set; }
        public bool IsHidden { get; set; }
        public String Url { get; set; }

        public IList<Fleet> Fleets { get; set; }
        public IList<Competitor> Competitors { get; set; }
        public IList<BoatClass> BoatClasses { get; set; }
        public IList<Season> Seasons { get; set; }
        public IList<Series> Series { get; set; }
        public IList<Race> Races { get; set; }
        public IList<Regatta> Regattas { get; set; }

        public string DefaultScoringSystemName { get; set; }
        public Guid? DefaultScoringSystemId { get; set; }

        public IList<ScoringSystem> ScoringSystems { get; set; }

        public IList<ScoringSystem> ScoringSystemOptions { get; set; }

        public IList<AdminToDoViewModel> Tips { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string TemperatureUnits { get; set; }
        public string SpeedUnits { get; set; }
        public IList<string> SpeedUnitOptions { get; set; }
        public IList<string> TemperatureUnitOptions { get; set; }
    }
}

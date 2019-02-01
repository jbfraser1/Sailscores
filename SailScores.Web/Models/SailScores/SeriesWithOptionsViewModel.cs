﻿using Microsoft.AspNetCore.Mvc.Rendering;
using SailScores.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SailScores.Web.Models.SailScores
{
    public class SeriesWithOptionsViewModel : Core.Model.Series
    {
        public IEnumerable<Season> SeasonOptions { get; set; }

        private Guid _seasonId;
        public Guid SeasonId
        {
            get
            {
                if (this.Season != null)
                {
                    return this.Season.Id;
                }
                return _seasonId;
            }
            set {
                _seasonId = value;
            }

        }
    }
}
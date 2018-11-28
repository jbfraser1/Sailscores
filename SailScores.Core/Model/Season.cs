﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sailscores.Core.Model
{
    public class Season
    {
        public Guid Id { get; set; }
        public Guid ClubId { get; set; }
        public Club Club { get; set; }

        [StringLength(200)]
        public String Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        
        public IEnumerable<Series> Series { get; set; }
    }
}

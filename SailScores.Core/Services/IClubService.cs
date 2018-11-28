﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Sailscores.Core.Model;

namespace Sailscores.Core.Services
{
    public interface IClubService
    {
        Task<IList<Model.Club>> GetClubs(bool includeHidden);
        Task<Club> GetFullClub(string id);
    }
}
﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SailScores.Core.Model;

namespace SailScores.Core.Services
{
    public interface IRegattaService
    {
        Task<IList<Model.Regatta>> GetAllRegattasAsync(
            Guid clubId);
        Task<Core.Model.Regatta> GetRegattaAsync(
            string clubInitials,
            string seasonName,
            string regattaName);
        Task SaveNewRegattaAsync(Regatta regatta);
        Task UpdateAsync(Regatta model);
        Task Delete(Guid regattaId);
        Task AddRaceToRegattaAsync(Race race, Guid regattaId);
    }
}
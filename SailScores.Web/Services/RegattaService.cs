﻿using AutoMapper;
using SailScores.Core.Model;
using SailScores.Core.Services;
using SailScores.Web.Models.SailScores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core = SailScores.Core;

namespace SailScores.Web.Services
{
    public class RegattaService : IRegattaService
    {
        private readonly Core.Services.IClubService _clubService;
        private readonly Core.Services.IRegattaService _coreRegattaService;
        private readonly IMapper _mapper;

        public RegattaService(
            Core.Services.IClubService clubService,
            Core.Services.IRegattaService coreRegattaService,
            IMapper mapper)
        {
            _clubService = clubService;
            _coreRegattaService = coreRegattaService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RegattaSummaryViewModel>> GetAllRegattaSummaryAsync(string clubInitials)
        {
            var coreObject = await _clubService.GetFullClub(clubInitials);
            var orderedRegattas = coreObject.Regattas
                .OrderByDescending(s => s.Season.Start)
                .ThenBy(s => s.StartDate)
                .ThenBy(s => s.Name);
            return _mapper.Map<IList<RegattaSummaryViewModel>>(orderedRegattas);
        }
        
        public async Task<IEnumerable<RegattaSummaryViewModel>> GetCurrentRegattas()
        {
            var start = DateTime.Today.AddDays(-7);
            var end = DateTime.Today.AddDays(7);

            var coreRegattas = await _coreRegattaService.GetRegattasDuringSpanAsync(start, end);
            var filteredRegattas = coreRegattas
                .OrderBy(s => s.StartDate)
                .ThenBy(s => s.Name);
            var vm = _mapper.Map<IList<RegattaSummaryViewModel>>(filteredRegattas);
            foreach(var regatta in vm)
            {
                var club = await _clubService.GetMinimalClub(regatta.ClubId);
                regatta.ClubInitials = club.Initials;
                regatta.ClubName = club.Name;
            }
            return vm;
        }

        public async Task<Regatta> GetRegattaAsync(Guid regattaId)
        {
            return await _coreRegattaService.GetRegattaAsync(regattaId);
        }

        public async Task<Regatta> GetRegattaAsync(string clubInitials, string season, string regattaName)
        {
            return await _coreRegattaService.GetRegattaAsync(clubInitials, season, regattaName);
        }

        public async Task<Guid> SaveNewAsync(RegattaWithOptionsViewModel model)
        {
            await PrepRegattaVmAsync(model);
            return await _coreRegattaService.SaveNewRegattaAsync(model);
        }

        private async Task PrepRegattaVmAsync(RegattaWithOptionsViewModel model)
        {
            var club = await _clubService.GetFullClub(model.ClubId);
            if (model.StartDate.HasValue)
            {
                model.Season = club.Seasons.Single(s => s.Start <= model.StartDate.Value
                && s.End >= model.StartDate.Value);
            }
            if (model.ScoringSystemId == Guid.Empty)
            {
                model.ScoringSystemId = null;
            }
            model.Fleets = new List<Fleet>();
            if (model.FleetIds != null)
            {
                foreach (var fleetId in model.FleetIds)
                {
                    model.Fleets.Add(club.Fleets
                        .Single(f => f.Id == fleetId));
                }
            }
        }

        public async Task<Guid> UpdateAsync(RegattaWithOptionsViewModel model)
        {
            await PrepRegattaVmAsync(model);
            return await _coreRegattaService.UpdateAsync(model);
        }

        public async Task DeleteAsync(Guid regattaId)
        {
            await _coreRegattaService.DeleteAsync(regattaId);
        }

        public async Task AddFleetToRegattaAsync(Guid fleetId, Guid regattaId)
        {
            await _coreRegattaService.AddFleetToRegattaAsync(fleetId, regattaId);
        }

    }
}
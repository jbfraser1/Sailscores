﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SailScores.Api.Dtos;
using SailScores.Core.Model;
using SailScores.Core.Services;
using SailScores.Web.Services;
using Model = SailScores.Core.Model;

namespace SailScores.Web.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class SeasonsController : ControllerBase
    {
        private readonly IClubService _clubService;
        private readonly IMapper _mapper;

        public SeasonsController(
            IClubService clubService,
            IMapper mapper)
        {
            _clubService = clubService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<SeasonDto>> Get(Guid clubId)
        {
            var club = await _clubService.GetFullClub(clubId);
            return _mapper.Map<List<SeasonDto>>(club.Seasons);
        }

        [HttpPost]
        public async Task<Guid> Post([FromBody] SeasonDto season)
        {
            var seasonBizObj = _mapper.Map<Season>(season);
            await _clubService.SaveNewSeason(seasonBizObj);
            var savedSeason =
                (await _clubService.GetFullClub(season.ClubId))
                .Seasons
                .First(c => c.Name == season.Name);
            return savedSeason.Id;
        }

    }
}
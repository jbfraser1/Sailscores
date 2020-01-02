﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SailScores.Core.Services;
using SailScores.Web.Models.SailScores;
using SailScores.Web.Services;

namespace SailScores.Web.Controllers
{
    public class RaceController : Controller
    {

        private readonly Core.Services.IClubService _clubService;
        private readonly Web.Services.IRaceService _raceService;
        private readonly Services.IAuthorizationService _authService;
        private readonly Services.IAdminTipService _adminTipService;
        private readonly IMapper _mapper;

        public RaceController(
            Core.Services.IClubService clubService,
            Web.Services.IRaceService raceService,
            Services.IAuthorizationService authService,
            Services.IAdminTipService adminTipService,
            IMapper mapper)
        {
            _clubService = clubService;
            _raceService = raceService;
            _authService = authService;
            _adminTipService = adminTipService;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index(string clubInitials,
            bool showScheduled = false,
            bool showAbandoned = true)
        {
            var races = await _raceService.GetAllRaceSummariesAsync(clubInitials,
                showScheduled,
                showAbandoned);

            return View(new ClubCollectionViewModel<RaceSummaryViewModel>
            {
                List = races,
                ClubInitials = clubInitials
            });
        }

        public async Task<ActionResult> Details(string clubInitials, Guid id)
        {
            var race = await _raceService.GetSingleRaceDetailsAsync(clubInitials, id);

            if(race == null)
            {
                return NotFound();
            }
            var canEdit = false;
            if(User != null && (User.Identity?.IsAuthenticated ?? false))
            {
                var clubId = await _clubService.GetClubId(clubInitials);
                canEdit = await _authService.CanUserEdit(User, clubId);
            }

            return View(new ClubItemViewModel<RaceViewModel>
            {
                Item = race,
                ClubInitials = clubInitials,
                CanEdit = canEdit
            });
        }

        [Authorize]
        public async Task<ActionResult> Create(
            string clubInitials,
            Guid? regattaId,
            Guid? seriesId,
            string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            RaceWithOptionsViewModel race =
                await _raceService.GetBlankRaceWithOptions(
                    clubInitials,
                    regattaId,
                    seriesId);
            var errors = _adminTipService.GetRaceCreateErrors(race);
            if(errors != null && errors.Count > 0)
            {
                return View("CreateErrors", errors);
            }
            _adminTipService.AddTips(ref race);
            return View(race);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create(
            string clubInitials,
            RaceWithOptionsViewModel race,
            string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            try
            {
                if (!ModelState.IsValid)
                {
                    RaceWithOptionsViewModel raceOptions =
                        await _raceService.GetBlankRaceWithOptions(
                            clubInitials,
                            race.RegattaId,
                            race.SeriesIds?.FirstOrDefault());
                    race.ScoreCodeOptions = raceOptions.ScoreCodeOptions;
                    race.FleetOptions = raceOptions.FleetOptions;
                    race.CompetitorBoatClassOptions = raceOptions.CompetitorBoatClassOptions;
                    race.CompetitorOptions = raceOptions.CompetitorOptions;
                    race.SeriesOptions = raceOptions.SeriesOptions;
                    return View(race);
                }
                var clubId = await _clubService.GetClubId(clubInitials);
                if (!await _authService.CanUserEdit(User, clubId))
                {
                    return Unauthorized();
                }
                race.ClubId = clubId;
                if (!ModelState.IsValid)
                {
                    var options = await _raceService.GetBlankRaceWithOptions(
                        clubInitials,
                        race.RegattaId,
                        null);
                    race.CompetitorBoatClassOptions = options.CompetitorBoatClassOptions;
                    race.CompetitorOptions = options.CompetitorOptions;
                    race.FleetOptions = options.FleetOptions;
                    race.ScoreCodeOptions = options.ScoreCodeOptions;
                    race.SeriesOptions = options.SeriesOptions;
                    race.WeatherIconOptions = options.WeatherIconOptions;
                    return View(race);
                }
                await _raceService.SaveAsync(race);
                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Admin");
            }
            catch
            {
                return View(race);
            }
        }

        [Authorize]
        public async Task<ActionResult> Edit(
            string clubInitials,
            Guid id,
            string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["ClubInitials"] = clubInitials;
            var clubId = await _clubService.GetClubId(clubInitials);
            if (!await _authService.CanUserEdit(User, clubId))
            {
                return Unauthorized();
            }
            var race = await _raceService.GetSingleRaceDetailsAsync(clubInitials, id);
            if (race == null)
            {
                return NotFound();
            }
            if (race.ClubId != clubId)
            {
                return Unauthorized();
            }

            var raceWithOptions = _mapper.Map<RaceWithOptionsViewModel>(race);

            await _raceService.AddOptionsToRace(raceWithOptions);

            return View(raceWithOptions);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            string clubInitials,
            Guid id,
            RaceWithOptionsViewModel race,
            string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["ClubInitials"] = clubInitials;
            try
            {
                if (!await _authService.CanUserEdit(User, race.ClubId))
                {
                    return Unauthorized();
                }
                if (!ModelState.IsValid)
                {
                    RaceWithOptionsViewModel raceOptions =
                        await _raceService.GetBlankRaceWithOptions(
                            clubInitials,
                            race.RegattaId,
                            race.SeriesIds?.FirstOrDefault());
                    race.ScoreCodeOptions = raceOptions.ScoreCodeOptions;
                    race.FleetOptions = raceOptions.FleetOptions;
                    race.CompetitorBoatClassOptions = raceOptions.CompetitorBoatClassOptions;
                    race.CompetitorOptions = raceOptions.CompetitorOptions;
                    race.SeriesOptions = raceOptions.SeriesOptions;
                    foreach(var score in race.Scores)
                    {
                        score.Competitor = raceOptions.CompetitorOptions.First(c => c.Id == score.CompetitorId);
                    }
                    return View(race);
                }
                await _raceService.SaveAsync(race);

                return RedirectToLocal(returnUrl);
            }
            catch
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Delete(string clubInitials, Guid id)
        {
            var clubId = await _clubService.GetClubId(clubInitials);
            if (!await _authService.CanUserEdit(User, clubId))
            {
                return Unauthorized();
            }
            var race = await _raceService.GetSingleRaceDetailsAsync(clubInitials, id);
            if (race == null)
            {
                return NotFound();
            }
            if (race.ClubId != clubId)
            {
                return Unauthorized();
            }
            return View(race);
        }

        [Authorize]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PostDelete(string clubInitials, Guid id)
        {
            try
            {
                var club = await _clubService.GetFullClub(clubInitials);
                if (!await _authService.CanUserEdit(User, club.Id)
                    || !club.Races.Any(c => c.Id == id))
                {
                    return Unauthorized();
                }
                await _raceService.Delete(id);

                return RedirectToAction("Index", "Admin");
            }
            catch
            {
                return View();
            }
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(AdminController.Index), "Admin");
            }
        }

    }
}
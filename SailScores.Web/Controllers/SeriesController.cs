using System;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SailScores.Identity.Entities;
using SailScores.Web.Models.SailScores;
using SailScores.Web.Services;

namespace SailScores.Web.Controllers
{
    public class SeriesController : Controller
    {

        private readonly Web.Services.ISeriesService _seriesService;
        private readonly Core.Services.IClubService _clubService;
        private readonly Services.IAuthorizationService _authService;
        private readonly Services.IAdminTipService _adminTipService;
        private readonly ICsvService _csvService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public SeriesController(
            Web.Services.ISeriesService seriesService,
            Core.Services.IClubService clubService,
            Services.IAuthorizationService authService,
            Services.IAdminTipService adminTipService,
            Services.ICsvService csvService,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _seriesService = seriesService;
            _clubService = clubService;
            _authService = authService;
            _adminTipService = adminTipService;
            _csvService = csvService;
            _userManager = userManager;
            _mapper = mapper;
        }

        // GET: Series
        [ResponseCache(Duration = 3600)]
        public async Task<ActionResult> Index(string clubInitials)
        {
            ViewData["ClubInitials"] = clubInitials.ToUpperInvariant();

            var series = await _seriesService.GetNonRegattaSeriesSummariesAsync(clubInitials);
            var clubName = await _clubService.GetClubName(clubInitials);

            return View(new ClubCollectionViewModel<SeriesSummary>
            {
                List = series,
                ClubInitials = clubInitials,
                ClubName = clubName,
                CanEdit = await _authService.CanUserEdit(User, clubInitials)
            });
        }

        public async Task<ActionResult> Details(
            string clubInitials,
            string season,
            string seriesName)
        {
            ViewData["ClubInitials"] = clubInitials;

            var series = await _seriesService.GetSeriesAsync(clubInitials, season, seriesName);
            if (series == null)
            {
                return new NotFoundResult();
            }
            var canEdit = false;
            if (User != null && (User.Identity?.IsAuthenticated ?? false))
            {
                var clubId = await _clubService.GetClubId(clubInitials);
                canEdit = await _authService.CanUserEdit(User, clubId);
            }

            return View(new ClubItemViewModel<Core.Model.Series>
            {
                Item = series,
                ClubInitials = clubInitials,
                CanEdit = canEdit
            });
        }

        public async Task<ActionResult> ExportCsv(
            string id)
        {

            var series = await _seriesService.GetSeriesAsync(new Guid(id));
            if (series == null)
            {
                return new NotFoundResult();
            }

            var filename = series.Name.Contains(series.Season.Name) ? $"{series.Name}.csv" : $"{series.Season.Name} {series.Name}.csv";
            var csv = _csvService.GetCsv(series);

            return File(csv, "text/csv", filename);
        }

        public async Task<ActionResult> ExportHtml(
            string id)
        {
            var series = await _seriesService.GetSeriesAsync(new Guid(id));
            if (series == null)
            {
                return new NotFoundResult();
            }
            var filename = series.Name.Contains(series.Season.Name) ? series.Name : $"{series.Season.Name} {series.Name}";
            // urlencode helps with unicode values, but replaces (valid) spaces.
            filename = HttpUtility.UrlEncode(filename + ".html", Encoding.UTF8);
            filename = filename.Replace("+", " ");
            var disposition = $"attachment; filename=\"{filename}\"; filename*=UTF-8''{filename}";
            Response.Headers.Add("content-disposition", disposition);

            return View(series);
        }

        public async Task<JsonResult> Chart(
            Guid seriesId)
        {
            var chartData = await _seriesService.GetChartData(seriesId);

            return Json(chartData);
        }


        [Authorize]
        public async Task<ActionResult> Create(string clubInitials)
        {
            var vm = await _seriesService.GetBlankVmForCreate(clubInitials);
            var errors = _adminTipService.GetSeriesCreateErrors(vm);
            if (errors != null && errors.Count > 0)
            {
                return View("CreateErrors", errors);
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create(string clubInitials, SeriesWithOptionsViewModel model)
        {
            try
            {
                var clubId = await _clubService.GetClubId(clubInitials);
                if (!await _authService.CanUserEdit(User, clubId))
                {
                    return Unauthorized();
                }
                model.ClubId = clubId;

                if (!ModelState.IsValid)
                {
                    var blankVm = await _seriesService.GetBlankVmForCreate(clubInitials);
                    model.SeasonOptions = blankVm.SeasonOptions;
                    model.ScoringSystemOptions = blankVm.ScoringSystemOptions;
                    return View(model);
                }

                model.UpdatedBy = await GetUserStringAsync();
                await _seriesService.SaveNew(model);

                return RedirectToAction("Index", "Admin");
            }
            catch
            {
                var blankVm = await _seriesService.GetBlankVmForCreate(clubInitials);
                model.SeasonOptions = blankVm.SeasonOptions;
                model.ScoringSystemOptions = blankVm.ScoringSystemOptions;

                ModelState.AddModelError(String.Empty,
                    "A problem occurred creating this series. Does a " +
                    "series with this season and this name already exist?");

                return View(model);
            }
        }

        private async Task<string> GetUserStringAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            return user.GetDisplayName();
        }

        [Authorize]
        public async Task<ActionResult> Edit(string clubInitials, Guid id)
        {
            var clubId = await _clubService.GetClubId(clubInitials);
            if (!await _authService.CanUserEdit(User, clubId))
            {
                return Unauthorized();
            }
            var series = await _seriesService.GetSeriesAsync(id);
            if (series == null)
            {
                return NotFound();
            }

            var seriesWithOptions = _mapper.Map<SeriesWithOptionsViewModel>(series);
            var blankVm = await _seriesService.GetBlankVmForCreate(clubInitials);
            seriesWithOptions.SeasonOptions = blankVm.SeasonOptions;
            seriesWithOptions.ScoringSystemOptions = blankVm.ScoringSystemOptions;
            return View(seriesWithOptions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit(string clubInitials, SeriesWithOptionsViewModel model)
        {
            try
            {
                var clubId = await _clubService.GetClubId(clubInitials);
                if (!await _authService.CanUserEdit(User, clubId))
                {
                    return Unauthorized();
                }
                model.ClubId = clubId;

                if (!ModelState.IsValid)
                {
                    var blankVm = await _seriesService.GetBlankVmForCreate(clubInitials);
                    model.SeasonOptions = blankVm.SeasonOptions;
                    model.ScoringSystemOptions = blankVm.ScoringSystemOptions;
                    return View(model);
                }

                model.UpdatedBy = await GetUserStringAsync();
                await _seriesService.Update(model);

                return RedirectToAction("Index", "Admin");
            }
            catch
            {
                return View(model);
            }
        }

        [Authorize]
        public async Task<ActionResult> Delete(string clubInitials, Guid id)
        {
            var clubId = await _clubService.GetClubId(clubInitials);
            if (!await _authService.CanUserEdit(User, clubId))
            {
                return Unauthorized();
            }
            var series = await _seriesService.GetSeriesAsync(id);
            if (series == null)
            {
                return NotFound();
            }
            return View(series);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> PostDelete(string clubInitials, Guid id)
        {
            var clubId = await _clubService.GetClubId(clubInitials);
            if (!await _authService.CanUserEdit(User, clubId))
            {
                return Unauthorized();
            }
            try
            {
                await _seriesService.DeleteAsync(id);

                return RedirectToAction("Index", "Admin");
            }
            catch
            {
                var series = await _seriesService.GetSeriesAsync(id);
                return View(series);
            }
        }
    }
}
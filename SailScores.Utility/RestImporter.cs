﻿using System;
using SwObjects = SailScores.ImportExport.Sailwave.Elements;
using System.Collections.Generic;
using System.Threading.Tasks;
using SailScores.Api.Services;
using SailScores.Api.Dtos;
using System.Linq;

namespace SailScores.Utility
{
    class RestImporter : IRestImporter
    {
        private readonly ISailScoresApiClient _apiClient;

        private ClubDto _club;
        private BoatClassDto _boatClass;
        private FleetDto _fleet;
        private SeasonDto _season;
        private SeriesDto _series;
        private IDictionary<int, CompetitorDto> _competitors;

        public RestImporter(
            ISailScoresApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task WriteSwSeriesToSS(SwObjects.Series series)
        {
            _club = await GetClub();
            _boatClass = await GetBoatClass();
            _fleet = await GetFleet();
            _season = await GetSeason();
            _series = await MakeSeries(series);
            _competitors = await GetCompetitors(series);
            await SaveRaces(series);
        }

        private async Task<ClubDto> GetClub()
        {
            var clubs = await _apiClient.GetClubsAsync();
            if (clubs.Count > 0)
            {
                Console.WriteLine($"There are {clubs.Count} clubs already in the database.");
                Console.Write("Would you like to use one of those? (Y / N) ");
                var result = Console.ReadLine();
                if (result.StartsWith("Y", StringComparison.InvariantCultureIgnoreCase))
                {
                    return SelectExistingClub(clubs);
                }
            }
            return await MakeNewClub();
        }

        private ClubDto SelectExistingClub(List<ClubDto> clubs)
        {
            int i = 1;
            foreach (var club in clubs)
            {
                Console.WriteLine($"{i++} - {club.Initials} : {club.Name}");
            }
            int result = 0;
            while (result == 0)
            {
                Console.Write("Enter a number of a club from above > ");
                var input = Console.ReadLine();
                Int32.TryParse(input, out result);
            }

            return clubs[result - 1];
        }

        private async Task<ClubDto> MakeNewClub()
        {
            Console.Write("Enter the new club name > ");
            var clubName = Console.ReadLine().Trim();
            Console.Write("Enter the club initials > ");
            var clubInitials = Console.ReadLine().Trim();

            var club = new ClubDto
            {
                Initials = clubInitials,
                Name = clubName
            };

            try
            {
                var guid = await _apiClient.SaveClub(club);
                club.Id = guid;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Oh Noes! There was an exception: {ex}");
                throw;
            }

            return club;
        }

        private async Task<BoatClassDto> GetBoatClass()
        {
            var boatClasses = await _apiClient.GetBoatClassesAsync(_club.Id);
            if (boatClasses.Count > 0)
            {
                Console.WriteLine($"There are {boatClasses.Count} boat classes already in the database.");
                Console.Write("Would you like to use one of those? (Y / N) ");
                var result = Console.ReadLine();
                if (result.StartsWith("Y", StringComparison.InvariantCultureIgnoreCase))
                {
                    return SelectExistingClass(boatClasses);
                }
            }
            return await MakeNewBoatClass();
        }

        private BoatClassDto SelectExistingClass(List<BoatClassDto> classes)
        {
            int i = 1;
            foreach (var boatClass in classes)
            {
                Console.WriteLine($"{i++} - {boatClass.Name}");
            }
            int result = 0;
            while (result == 0)
            {
                Console.Write("Enter a number of a class from above > ");
                var input = Console.ReadLine();
                Int32.TryParse(input, out result);
            }

            return classes[result - 1];
        }


        private async Task<BoatClassDto> MakeNewBoatClass()
        {
            Console.Write("Enter the new class name > ");
            var className = Console.ReadLine().Trim();

            var boatClass = new BoatClassDto
            {
                Name = className,
                ClubId = _club.Id
            };

            try
            {
                var guid = await _apiClient.SaveBoatClass(boatClass);
                boatClass.Id = guid;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Oh Noes! There was an exception: {ex}");
                throw;
            }

            return boatClass;
        }


        private async Task<FleetDto> GetFleet()
        {
            var fleets = await _apiClient.GetFleetsAsync(_club.Id);
            fleets = fleets.Where(f =>
                    f.FleetType == Api.Enumerations.FleetType.AllBoatsInClub
                    || f.FleetType == Api.Enumerations.FleetType.SelectedBoats
                    || (f.FleetType == Api.Enumerations.FleetType.SelectedClasses
                        && f.BoatClassIds.Contains(_boatClass.Id)))
                        .OrderBy(f => f.Name)
                        .ToList();
            if (fleets.Count > 0)
            {
                Console.WriteLine($"There are {fleets.Count} compatible fleets already in this club:");
                foreach (var fleet in fleets)
                {
                    Console.WriteLine(fleet.Name);
                }
                Console.Write("Would you like to use one of those? (Y / N) ");
                var result = Console.ReadLine();
                if (result.StartsWith("Y", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (fleets.Count > 1)
                    {
                        return SelectExistingFleet(fleets);
                    }
                    else
                    {
                        return fleets[0];
                    }
                }
            }
            return await MakeNewFleet();
        }

        private FleetDto SelectExistingFleet(List<FleetDto> fleets)
        {
            int i = 1;
            foreach (var fleet in fleets)
            {
                Console.WriteLine($"{i++} - {fleet.Name}");
            }
            int result = 0;
            while (result == 0)
            {
                Console.Write("Enter a number of a fleet from above > ");
                var input = Console.ReadLine();
                Int32.TryParse(input, out result);
            }

            return fleets[result - 1];
        }


        private async Task<FleetDto> MakeNewFleet()
        {
            Console.Write("Enter the new fleet name > ");
            var className = Console.ReadLine().Trim();
            Console.Write("Enter the new fleet short name / abbreviation > ");
            var shortName = Console.ReadLine().Trim();

            var fleet = new FleetDto
            {
                Name = className,
                ShortName = shortName,
                ClubId = _club.Id,
                FleetType = Api.Enumerations.FleetType.SelectedBoats
            };

            try
            {
                var guid = await _apiClient.SaveFleet(fleet);
                fleet.Id = guid;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Oh Noes! There was an exception: {ex}");
                throw;
            }

            return fleet;
        }

        private static int GetYear()
        {
            Console.Write("What year was this series? > ");
            var result = 0;
            while (result < 1900)
            {
                var input = Console.ReadLine();
                Int32.TryParse(input, out result);
            }
            return result;
        }

        private async Task<SeasonDto> GetSeason()
        {
            var seasons = await _apiClient.GetSeasonsAsync(_club.Id);
            if (seasons.Count > 0)
            {
                int i = 1;
                foreach (var season in seasons)
                {
                    Console.WriteLine($"{i++} - {season.Name}: {season.Start}-{season.End}");
                }
                int result = -1;
                while (result == -1)
                {
                    Console.WriteLine($"There are {seasons.Count} seasons already in the database.");
                    Console.Write("Enter a number of a season from above or 0 to create a new season > ");
                    var input = Console.ReadLine();
                    Int32.TryParse(input, out result);
                }
                if (result > 0)
                {
                    return seasons[result - 1];
                }
            }

            return await MakeNewSeason();
        }

        private async Task<SeasonDto> MakeNewSeason()
        {
            Console.Write("Enter the new season name > ");
            var className = Console.ReadLine().Trim();
            Console.Write("The season will be created for the full year. ");
            var year = GetYear();
            var start = new DateTime(year, 1, 1);
            var season = new SeasonDto
            {
                Name = className,
                Start = start,
                End = start.AddYears(1),
                ClubId = _club.Id
            };

            try
            {
                var guid = await _apiClient.SaveSeason(season);
                season.Id = guid;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Oh Noes! There was an exception: {ex}");
                throw;
            }

            return season;
        }

        private async Task<SeriesDto> MakeSeries(SwObjects.Series series)
        {
            Console.Write("What is the name of this series? > ");
            var result = Console.ReadLine();
            var seriesDto = new SeriesDto
            {
                ClubId = _club.Id,
                Name = result,
                SeasonId = _season.Id
            };

            try
            {
                var guid = await _apiClient.SaveSeries(seriesDto);
                seriesDto.Id = guid;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Oh Noes! There was an exception: {ex}");
                throw;
            }
            return seriesDto;
        }

        private async Task<IDictionary<int, CompetitorDto>> GetCompetitors(SwObjects.Series series)
        {

            List<CompetitorDto> competitors;
            if (_fleet.FleetType == Api.Enumerations.FleetType.SelectedBoats)
            {
                competitors = await _apiClient.GetCompetitors(_club.Id, null);
            }
            else
            {
                competitors = await _apiClient.GetCompetitors(_club.Id, _fleet.Id);
            }

            List<CompetitorDto> existingCompetitors = GetMatchedCompetitors(competitors, series);
            foreach (var comp in existingCompetitors)
            {
                if (!(comp.FleetIds.Contains(_fleet.Id)))
                {
                    comp.FleetIds.Add(_fleet.Id);
                    await _apiClient.SaveCompetitor(comp);
                }
            }

            List<CompetitorDto> competitorsToCreate = GetCompetitorsToCreate(competitors, series);


            foreach (var comp in competitorsToCreate)
            {
                comp.FleetIds = new List<Guid>
                {
                    _fleet.Id
                };
                await _apiClient.SaveCompetitor(comp);
            }

            var shouldBeAllCompetitors = await _apiClient.GetCompetitors(_club.Id, _fleet.Id);

            // build the dictionary
            var returnDict = new Dictionary<int, CompetitorDto>();
            foreach (var swComp in series.Competitors)
            {
                if (series.Races.SelectMany(r => r.Results)
                    .Any(r => r.CompetitorId == swComp.Id
                        && (r.Code != null || r.Place != 0)))
                {
                    var match = FindMatch(shouldBeAllCompetitors, swComp);
                    if (match == null)
                    {
                        throw new InvalidOperationException("Failed to find a competitor that should have just been created.");
                    }
                    returnDict.Add(swComp.Id, FindMatch(shouldBeAllCompetitors, swComp));
                }
            }

            return returnDict;
        }


        private List<CompetitorDto> GetMatchedCompetitors(List<CompetitorDto> competitors, SwObjects.Series series)
        {
            var returnList = new List<CompetitorDto>();
            foreach (var comp in series.Competitors)
            {
                if (series.Races.SelectMany(r => r.Results)
                    .Any(r => r.CompetitorId == comp.Id
                    && (r.Code != null || r.Place != 0)))
                {
                    var match = FindMatch(competitors, comp);
                    if (match != null)
                    {
                        returnList.Add(match);
                    }
                }
            }
            return returnList;
        }

        private List<CompetitorDto> GetCompetitorsToCreate(List<CompetitorDto> competitors, SwObjects.Series series)
        {
            var returnList = new List<CompetitorDto>();
            foreach (var comp in series.Competitors)
            {
                if (series.Races.SelectMany(r => r.Results)
                    .Any(r => r.CompetitorId == comp.Id
                    && (r.Code != null || r.Place != 0))
                    && FindMatch(competitors, comp) == null)
                {
                    returnList.Add(
                        new CompetitorDto
                        {
                            Name = comp.HelmName,
                            BoatName = comp.Boat,
                            SailNumber = comp.SailNumber,
                            BoatClassId = _boatClass.Id,
                            ClubId = _club.Id
                        });
                }
            }
            return returnList;
        }

        private static CompetitorDto FindMatch(
            List<CompetitorDto> competitors,
            SwObjects.Competitor c)
        {
            foreach (var comp in competitors)
            {
                if ((!String.IsNullOrWhiteSpace(c.SailNumber)
                    && c.SailNumber == comp.SailNumber)
                    || (!String.IsNullOrWhiteSpace(c.SailNumber)
                        && c.SailNumber == comp.AlternativeSailNumber))
                {
                    return comp;
                }
                if (String.IsNullOrWhiteSpace(c.SailNumber)
                    && String.IsNullOrWhiteSpace(comp.SailNumber)
                    && c.HelmName == comp.Name)
                {
                    return comp;
                }
            }
            return null;
        }

        private async Task SaveRaces(SwObjects.Series series)
        {
            var races = MakeRaces(series);

            await PostRaces(races);

        }


        private IList<RaceDto> MakeRaces(SwObjects.Series series)
        {
            var retList = new List<RaceDto>();

            foreach (var swRace in series.Races)
            {
                DateTime date = GetDate(swRace, _season);
                int rank = GetRaceRank(swRace);
                var ssRace = new RaceDto
                {
                    Name = swRace.Name,
                    Order = rank,
                    ClubId = _club.Id,
                    Date = date,
                    FleetId = _fleet.Id,
                    SeriesIds = new List<Guid> { _series.Id }
                };
                ssRace.Scores = MakeScores(swRace, series.Competitors, _boatClass, _fleet);

                retList.Add(ssRace);
            }
            return retList;
        }

        private IList<ScoreDto> MakeScores(
            SwObjects.Race swRace,
            List<SwObjects.Competitor> competitors,
            BoatClassDto boatClass,
            FleetDto fleet)
        {
            var retList = new List<ScoreDto>();
            foreach (var swScore in swRace.Results)
            {
                if (String.IsNullOrWhiteSpace(swScore.Code)
                    && swScore.Place == 0)
                {
                    continue;
                }
                // Not going to import DNCs.
                // Sailwave can leave DNC in some codes when type is changed to scored.
                if (swScore.Code == "DNC" && swScore.ResultType == 3)
                {
                    continue;
                }

                var score = new ScoreDto
                {
                    Place = swScore.Place,
                    Code = swScore.ResultType == 3 ? swScore.Code : null
                };
                var swComp = competitors.Single(c => c.Id == swScore.CompetitorId);
                score.CompetitorId = _competitors[swComp.Id].Id;

                retList.Add(score);
            }

            return retList;
        }

        private async Task PostRaces(IList<RaceDto> races)
        {
            foreach (var race in races)
            {
                await _apiClient.SaveRace(race);
            }
        }

        private static DateTime GetDate(SwObjects.Race swRace, SeasonDto season)
        {
            // assume race name format of "6-22 R1" or "6-23"
            var datepart = swRace.Name.Split(' ')[0];
            if (String.IsNullOrWhiteSpace(datepart))
            {
                return DateTime.Today;
            }
            var parts = datepart.Split(new[] { '-', '/' });
            int month = season.Start.Month;
            int day = season.Start.Day;
            Int32.TryParse(parts[0], out month);
            Int32.TryParse(parts[1], out day);
            return new DateTime(season.Start.Year, month, day);
        }

        private static int GetRaceRank(SwObjects.Race swRace)
        {
            // assume race name format of "6-22 R1" or "6-23"
            var parts = swRace.Name.Split('R');
            if (parts.Length < 2)
            {
                return 1;
            }
            var numberString = new String(parts[1].Where(Char.IsDigit).ToArray());
            int rank = swRace.Rank;
            Int32.TryParse(numberString, out rank);
            return rank;
        }


    }
}

﻿using AutoMapper;
using SailScores.Core.Services;
using SailScores.Database;
using SailScores.Test.Unit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SailScores.Core.Model;
using Xunit;

namespace SailScores.Test.Unit.Core.Services
{
    public class FleetServiceTests
    {
        FleetService _service;

        private readonly ISailScoresContext _context;
        private readonly Guid _clubId;
        private readonly IMapper _mapper;

        public FleetServiceTests()
        {
            _context = InMemoryContextBuilder.GetContext();
            _clubId = _context.Clubs.First().Id;
            _mapper = MapperBuilder.GetSailScoresMapper();
            _service = new FleetService(
                _context,
                _mapper);
        }

        [Fact]
        public async Task SaveNew_Always_SavesToDb()
        {
            var startingFleetCount = _context.Fleets.Count();

            var newFleet = new Fleet
            {
                Name = "myFleet",
                ShortName = "myFleet",
                NickName = "myFleet",
            };

            await _service.SaveNew(newFleet);

            Assert.NotEmpty(_context.Fleets
                .Where(f => f.Name == newFleet.Name));
            Assert.Equal(startingFleetCount + 1,
                _context.Fleets.Count());
        }

        [Fact]
        public async Task Delete_Fleet_RemovesFromDb()
        {
            // Arrange
            var boatClass = await _context.BoatClasses.FirstAsync();
            var comp = await _context.Competitors.FirstAsync();
            var newFleet = new Fleet
            {
                Name = "myFleet",
                ShortName = "myFleet",
                NickName ="myFleet",
                BoatClasses = new List<BoatClass>
                {
                    new BoatClass
                    {
                        Id = boatClass.Id,
                        ClubId = boatClass.ClubId,
                        Name = boatClass.Name
                    }
                },
                Competitors = new List<Competitor>
                {
                    new Competitor
                    {
                        Id = comp.Id,
                        ClubId = comp.ClubId,
                        Name = comp.Name
                    }
                }
            };

            await _service.SaveNew(newFleet);

            Assert.NotEmpty(_context.Fleets
                .Where(f => f.Name == newFleet.Name).SelectMany(
                f => f.FleetBoatClasses));

            var newFleetId = _context.Fleets
                .Where(f => f.Name == newFleet.Name).First().Id;

            //Act 
            await _service.Delete(newFleetId);

            // Assert
            Assert.Empty(_context.Fleets
                .Where(f => f.Name == newFleet.Name));

        }


        [Fact]
        public async Task Get_Fleet_ReturnsFromDb()
        {
            // Arrange
            var boatClass = await _context.BoatClasses.FirstAsync();
            var newFleet = new Fleet
            {
                Name = "myFleet",
                ShortName = "myFleet",
                NickName = "myFleet",
                BoatClasses = new List<BoatClass>
                {
                    new BoatClass
                    {
                        Id = boatClass.Id,
                        ClubId = boatClass.ClubId,
                        Name = boatClass.Name
                    }
                }

            };

            await _service.SaveNew(newFleet);

            Assert.NotEmpty(_context.Fleets
                .Where(f => f.Name == newFleet.Name).SelectMany(
                f => f.FleetBoatClasses));

            var newFleetId = _context.Fleets
                .Where(f => f.Name == newFleet.Name).First().Id;

            //Act 
            var testresult = await _service.Get(newFleetId);

            // Assert
            Assert.Equal(newFleet.Name, testresult.Name);

        }

        [Fact]
        public async Task GetAllFleetsForClub_ReturnFromDb()
        {
            // Arrange

            // Act
            var fleets = await _service.GetAllFleetsForClub(_clubId);

            // Assert
            Assert.NotEmpty(fleets);

        }

        [Fact]
        public async Task GetSeriesForFleet_returnsFromDb()
        {
            //Arrange
            var race = await _context.Races.FirstAsync();
            var fleet = await _context.Fleets.FirstAsync();
            var series = await _context.Series.FirstAsync();

            race.Fleet = fleet;
            series.RaceSeries = new List<Database.Entities.SeriesRace>
            {
                new Database.Entities.SeriesRace
                {
                    RaceId = race.Id,
                    SeriesId = series.Id

                }
            };
            await _context.SaveChangesAsync();

            // Act
            var returnedValue = await _service.GetSeriesForFleet(fleet.Id) ;

            // Assert
            Assert.NotEmpty(returnedValue);
        }
    }
}

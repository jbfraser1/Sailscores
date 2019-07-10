﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SailScores.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using System.Linq;
using SailScores.Core.Model;
using Db = SailScores.Database.Entities;

namespace SailScores.Core.Services
{
    public class ScoringService : IScoringService
    {
        private readonly ISailScoresContext _dbContext;
        private readonly IMapper _mapper;

        public ScoringService(
            ISailScoresContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Model.ScoreCode>> GetScoreCodesAsync(Guid clubId)
        {
            var scoreCodes = await _dbContext
                .ScoringSystems
                .Where(s => s.ClubId == clubId || s.ClubId == null)
                .SelectMany(s => s.ScoreCodes)
                .GroupBy(s => s.Name, (key, g) => g.OrderBy(e => e.Name).First())
                .ToListAsync();

            return _mapper.Map<List<Model.ScoreCode>>(scoreCodes);
        }

        public async Task<IList<Model.ScoringSystem>> GetScoringSystemsAsync(
            Guid clubId,
            bool includeBaseSystems)
        {
            var dbObjects = await _dbContext
                .ScoringSystems
                .Where(s => s.ClubId == clubId ||
                (includeBaseSystems && s.ClubId == null))
                .Include(s => s.ScoreCodes)
                .ToListAsync();
            return _mapper.Map<List<Model.ScoringSystem>>(dbObjects);
        }

        // This is a big one: returns a scoring system, including inherited ScoreCodes.
        // So all score codes that will work in this scoring system will be returned.
        public async Task<ScoringSystem> GetScoringSystemAsync(Guid scoringSystemId)
        {
            var requestedDbSystem = await _dbContext
                .ScoringSystems
                .Include(s => s.ScoreCodes)
                .SingleAsync(s => s.Id == scoringSystemId);

            var requestedSystem = _mapper.Map<Model.ScoringSystem>(requestedDbSystem);

            // Should inherited codes include the codes that are overriden in this system?
            // For now, going to say no.  but the call below does include those, so
            // need to remove them
            var allInheritedCodes = await GetAllCodesAsync(requestedSystem.ParentSystemId);
            requestedSystem.InheritedScoreCodes = allInheritedCodes
                .Where(c => !requestedSystem.ScoreCodes.Any(ec => ec.Name == c.Name));

            return requestedSystem;

        }

        public async Task<ScoringSystem> GetScoringSystemAsync(Series series)
        {
            Guid? scoringSystemId = series.ScoringSystemId;
            if(scoringSystemId == null)
            {
                scoringSystemId = (await _dbContext.Clubs.Where(c => c.Id == series.ClubId)
                    .FirstOrDefaultAsync())
                    .DefaultScoringSystemId;
            }
            if(scoringSystemId == null)
            {
                throw new InvalidOperationException("Scoring system for series not found and club default scoring system not found.");
            }
            return await GetScoringSystemAsync(scoringSystemId.Value);
        }

        public async Task<ScoreCode> GetScoreCodeAsync(Guid id)
        {
            var dbObj = await _dbContext.ScoreCodes.Where(sc => sc.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<ScoreCode>(dbObj);
        }

        public async Task SaveScoreCodeAsync(ScoreCode scoreCode)
        {
            if(scoreCode.Id != null && scoreCode.Id != Guid.Empty)
            {
                Db.ScoreCode dbObject = await _dbContext.ScoreCodes.Where(sc => sc.Id == scoreCode.Id).SingleAsync();
                _mapper.Map(scoreCode, dbObject);
            } else
            {
                Db.ScoreCode dbObject = _mapper.Map<Db.ScoreCode>(scoreCode);
                dbObject.Id = Guid.NewGuid();
                _dbContext.ScoreCodes.Add(dbObject);
            }
            await _dbContext.SaveChangesAsync();
        }

        private async Task<IEnumerable<ScoreCode>> GetAllCodesAsync(
            Guid? systemId)
        {
            if(systemId == null)
            {
                return new List<Model.ScoreCode>();
            }
            var requestedDbSystem = await _dbContext
                .ScoringSystems
                .Include(s => s.ScoreCodes)
                .SingleAsync(s => s.Id == systemId);
            var thisSystemCodes = _mapper.Map<IList<ScoreCode>>(requestedDbSystem.ScoreCodes);

            var parentCodes = await GetAllCodesAsync(requestedDbSystem.ParentSystemId);
            var parentCodesToUse = parentCodes
                    .Where(pc => !requestedDbSystem.ScoreCodes.Any(cc => cc.Name == pc.Name))
                    .ToList();

            var returnCodes = thisSystemCodes.Concat(parentCodesToUse);

            return returnCodes;
        }

    }
}

﻿using SailScores.ImportExport.Sailwave;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SailScores.ImportExport.Sailwave.Elements;
using SailScores.ImportExport.Sailwave.Parsers;
using SailScores.ImportExport.Sailwave.Writers;
using Xunit;

namespace SailScores.ImportExport.Sailwave.Tests.Unit
{
    public class WriterTests
    {
        private Series _series;

        public WriterTests()
        {
            _series = Utilities.SimplePocos.GetSeries();
        }

        [Fact]
        public async void BasicWriteFile()
        {
            var rows = await SeriesWriter.WriteSeries(_series);
            
            Assert.True( rows.Count() > 10 );

        }

        [Fact]
        public async void WriteFile_GetsLotsOfDetails()
        {
            var rows = await SeriesWriter.WriteSeries(_series);
            
            Assert.True(rows.Count(r => r.Name.StartsWith("ser")) > 20);

        }

        [Fact]
        public async void WriteFile_HasScoreRows()
        {
            var rows = await SeriesWriter.WriteSeries(_series);

            Assert.True(rows.Count(r => r.Name.StartsWith("scr")) > 20);
        }

        [Fact]
        public async void WriteFile_AllScoreRowsHaveSameScoringSystemId()
        {
            var rows = await SeriesWriter.WriteSeries(_series);

            Assert.Single(rows.Where(r => r.Name.StartsWith("scr")
                        && r.CompetitorOrScoringSystemId.HasValue)
                .Select(r => r.CompetitorOrScoringSystemId)
                .Distinct());
        }
        [Fact]
        public async void WriteFile_scrtiefinalsIsYesOrNo()
        {
            var rows = await SeriesWriter.WriteSeries(_series);
            var rowToCheck = rows.First(r => r.Name.Equals("scrtiefinals"));
            
            Assert.True(rowToCheck.Value.Equals("Yes", StringComparison.InvariantCultureIgnoreCase)
                || rowToCheck.Value.Equals("No", StringComparison.InvariantCultureIgnoreCase));
        }

        [Fact]
        public async void WriteFile_scrfollowchampIs1or0()
        {
            var rows = await SeriesWriter.WriteSeries(_series);
            var rowToCheck = rows.First(r => r.Name.Equals("scrfollowchamp"));

            Assert.True(rowToCheck.Value.Equals("1", StringComparison.InvariantCultureIgnoreCase)
                        || rowToCheck.Value.Equals("0", StringComparison.InvariantCultureIgnoreCase));
        }

        [Fact]
        public async void WriteFile_HasScoreCodeRowsWithId()
        {
            var rows = await SeriesWriter.WriteSeries(_series);

            var scrcodeRow = rows.Count(r => r.Name.Equals("scrcode"));
            var scrcodeRowsWithId = rows.Count(r => r.Name.Equals("scrcode")
            && r.Value.Contains("|5|"));
            Assert.True(scrcodeRow > 2);
            Assert.Equal(scrcodeRow, scrcodeRowsWithId);
        }

        [Fact]
        public async void WriteFile_ScoreCodeRowsHaveNoTrueId()
        {
            var rows = await SeriesWriter.WriteSeries(_series);
            
            var scrcodeRowsWithTrueId = rows.Count(r => r.Name.Equals("scrcode")
                                                    && r.CompetitorOrScoringSystemId.HasValue);

            Assert.Equal(0, scrcodeRowsWithTrueId);
        }

        [Fact]
        public async void WriteFile_HasOneUiRow()
        {
            var rows = await SeriesWriter.WriteSeries(_series);

            var uiRowCount = rows.Count(r => r.Name.Equals("ui"));
            Assert.Equal(1, uiRowCount);
        }

        [Fact]
        public async void WriteFile_HasCompetitorRowsWithTwoIds()
        {
            var rows = await SeriesWriter.WriteSeries(_series);

            Assert.Equal(2, rows.Where(r => r.Name.StartsWith("comp")
                                            && r.CompetitorOrScoringSystemId.HasValue)
                .Select(r => r.CompetitorOrScoringSystemId)
                .Distinct()
                .Count());
        }

        [Fact]
        public async void WriteFile_HasTwoRaces()
        {
            var rows = await SeriesWriter.WriteSeries(_series);

            Assert.Equal(2, rows.Where(r => r.Name.StartsWith("race")
                                            && r.RaceId.HasValue)
                .Select(r => r.RaceId)
                .Distinct()
                .Count());
        }


        [Fact]
        public async void WriteFile_HasRaceResultRowsForAllCompetitorsInAllRaces()
        {
            var rows = await SeriesWriter.WriteSeries(_series);

            Assert.Equal(4, rows.Where(r => r.CompetitorOrScoringSystemId.HasValue
                                            && r.RaceId.HasValue)
                .Select(r => new {
                    r.RaceId,
                    r.CompetitorOrScoringSystemId
                })
                .Distinct()
                .Count());
        }

        [Fact]
        public async void WriteFile_HasLotsOfColumnRows()
        {
            var rows = await SeriesWriter.WriteSeries(_series);

            Assert.True(rows.Count(r => r.Name == "column") > 100);
        }
    }
}

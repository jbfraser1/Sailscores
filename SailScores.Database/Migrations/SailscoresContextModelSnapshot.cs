﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SailScores.Database;

namespace SailScores.Database.Migrations
{
    [DbContext(typeof(SailScoresContext))]
    partial class SailScoresContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SailScores.Database.Entities.BoatClass", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClubId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(2000)")
                        .HasMaxLength(2000);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.ToTable("BoatClasses");
                });

            modelBuilder.Entity("SailScores.Database.Entities.Club", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("DefaultScoringSystemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Initials")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<bool>("IsHidden")
                        .HasColumnType("bit");

                    b.Property<string>("Locale")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<bool?>("ShowClubInResults")
                        .HasColumnType("bit");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("WeatherSettingsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("DefaultScoringSystemId");

                    b.HasIndex("WeatherSettingsId");

                    b.ToTable("Clubs");
                });

            modelBuilder.Entity("SailScores.Database.Entities.ClubRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdminNotes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Classes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClubInitials")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("ClubLocation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClubName")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("ClubWebsite")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("ForTesting")
                        .HasColumnType("bit");

                    b.Property<bool?>("Hide")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("RequestApproved")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("RequestSubmitted")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("TestClubId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TypicalDiscardRules")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("VisibleClubId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("ClubRequests");
                });

            modelBuilder.Entity("SailScores.Database.Entities.Competitor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AlternativeSailNumber")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<Guid>("BoatClassId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BoatName")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<Guid>("ClubId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("HomeClubName")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(2000)")
                        .HasMaxLength(2000);

                    b.Property<string>("SailNumber")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.HasIndex("BoatClassId");

                    b.HasIndex("ClubId");

                    b.ToTable("Competitors");
                });

            modelBuilder.Entity("SailScores.Database.Entities.CompetitorFleet", b =>
                {
                    b.Property<Guid>("CompetitorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FleetId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CompetitorId", "FleetId");

                    b.HasIndex("FleetId");

                    b.ToTable("CompetitorFleet");
                });

            modelBuilder.Entity("SailScores.Database.Entities.CompetitorRankStats", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Count")
                        .HasColumnType("int");

                    b.Property<int?>("Place")
                        .HasColumnType("int");

                    b.Property<string>("SeasonName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SeasonStart")
                        .HasColumnType("datetime2");

                    b.ToTable("CompetitorRankStats");
                });

            modelBuilder.Entity("SailScores.Database.Entities.CompetitorStatsSummary", b =>
                {
                    b.Property<double?>("AverageFinishRank")
                        .HasColumnType("float");

                    b.Property<int?>("BoatsBeat")
                        .HasColumnType("int");

                    b.Property<int?>("BoatsRacedAgainst")
                        .HasColumnType("int");

                    b.Property<int?>("DaysRaced")
                        .HasColumnType("int");

                    b.Property<int>("RaceCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("SeasonEnd")
                        .HasColumnType("datetime2");

                    b.Property<string>("SeasonName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SeasonStart")
                        .HasColumnType("datetime2");

                    b.ToTable("CompetitorStatsSummary");
                });

            modelBuilder.Entity("SailScores.Database.Entities.File", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("FileContents")
                        .HasColumnType("varbinary(max)");

                    b.Property<DateTime?>("ImportedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("SailScores.Database.Entities.Fleet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClubId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(2000)")
                        .HasMaxLength(2000);

                    b.Property<int>("FleetType")
                        .HasColumnType("int");

                    b.Property<bool>("IsHidden")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("NickName")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("ShortName")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.ToTable("Fleets");
                });

            modelBuilder.Entity("SailScores.Database.Entities.FleetBoatClass", b =>
                {
                    b.Property<Guid>("FleetId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BoatClassId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FleetId", "BoatClassId");

                    b.HasIndex("BoatClassId");

                    b.ToTable("FleetBoatClass");
                });

            modelBuilder.Entity("SailScores.Database.Entities.HistoricalResults", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<bool>("IsCurrent")
                        .HasColumnType("bit");

                    b.Property<string>("Results")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SeriesId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SeriesId");

                    b.ToTable("HistoricalResults");
                });

            modelBuilder.Entity("SailScores.Database.Entities.Race", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClubId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<Guid?>("FleetId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("TrackingUrl")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnName("UpdatedDateUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("WeatherId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.HasIndex("FleetId");

                    b.HasIndex("WeatherId");

                    b.ToTable("Races");
                });

            modelBuilder.Entity("SailScores.Database.Entities.Regatta", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClubId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<bool?>("PreferAlternateSailNumbers")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ScoringSystemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SeasonId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnName("UpdatedDateUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("UrlName")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.HasIndex("ScoringSystemId");

                    b.HasIndex("SeasonId");

                    b.ToTable("Regattas");
                });

            modelBuilder.Entity("SailScores.Database.Entities.RegattaFleet", b =>
                {
                    b.Property<Guid>("RegattaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FleetId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RegattaId", "FleetId");

                    b.HasIndex("FleetId");

                    b.ToTable("RegattaFleet");
                });

            modelBuilder.Entity("SailScores.Database.Entities.RegattaSeries", b =>
                {
                    b.Property<Guid>("RegattaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SeriesId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RegattaId", "SeriesId");

                    b.HasIndex("SeriesId");

                    b.ToTable("RegattaSeries");
                });

            modelBuilder.Entity("SailScores.Database.Entities.Score", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<decimal?>("CodePoints")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<Guid>("CompetitorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Place")
                        .HasColumnType("int");

                    b.Property<Guid>("RaceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CompetitorId");

                    b.HasIndex("RaceId");

                    b.ToTable("Scores");
                });

            modelBuilder.Entity("SailScores.Database.Entities.ScoreCode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool?>("AdjustOtherScores")
                        .HasColumnType("bit");

                    b.Property<bool?>("CameToStart")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<bool?>("Discardable")
                        .HasColumnType("bit");

                    b.Property<bool?>("Finished")
                        .HasColumnType("bit");

                    b.Property<string>("Formula")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int?>("FormulaValue")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<bool?>("PreserveResult")
                        .HasColumnType("bit");

                    b.Property<string>("ScoreLike")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ScoringSystemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool?>("Started")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ScoringSystemId");

                    b.ToTable("ScoreCodes");
                });

            modelBuilder.Entity("SailScores.Database.Entities.ScoringSystem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ClubId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DiscardPattern")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<Guid?>("OwningClubId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ParentSystemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal?>("ParticipationPercent")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.HasIndex("ParentSystemId");

                    b.ToTable("ScoringSystems");
                });

            modelBuilder.Entity("SailScores.Database.Entities.Season", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClubId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("End")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<DateTime>("Start")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("SailScores.Database.Entities.Series", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClubId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(2000)")
                        .HasMaxLength(2000);

                    b.Property<bool?>("ExcludeFromCompetitorStats")
                        .HasColumnType("bit");

                    b.Property<Guid?>("FleetId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool?>("IsImportantSeries")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<bool?>("PreferAlternativeSailNumbers")
                        .HasColumnType("bit");

                    b.Property<bool?>("ResultsLocked")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ScoringSystemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SeasonId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TrendOption")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnName("UpdatedDateUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("UrlName")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.HasIndex("ScoringSystemId");

                    b.HasIndex("SeasonId");

                    b.ToTable("Series");
                });

            modelBuilder.Entity("SailScores.Database.Entities.SeriesChartResults", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<bool>("IsCurrent")
                        .HasColumnType("bit");

                    b.Property<string>("Results")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SeriesId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SeriesId");

                    b.ToTable("SeriesChartResults");
                });

            modelBuilder.Entity("SailScores.Database.Entities.SeriesRace", b =>
                {
                    b.Property<Guid>("SeriesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RaceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("SeriesId", "RaceId");

                    b.HasIndex("RaceId");

                    b.ToTable("SeriesRace");
                });

            modelBuilder.Entity("SailScores.Database.Entities.UserClubPermission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("CanEditAllClubs")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ClubId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserEmail")
                        .HasColumnType("nvarchar(254)")
                        .HasMaxLength(254);

                    b.HasKey("Id");

                    b.ToTable("UserPermissions");
                });

            modelBuilder.Entity("SailScores.Database.Entities.Weather", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal?>("CloudCoverPercent")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnName("CreatedDateUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<decimal?>("Humidity")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("Icon")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<decimal?>("TemperatureDegreesKelvin")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("TemperatureString")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<decimal?>("WindDirectionDegrees")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("WindDirectionString")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<decimal?>("WindGustMeterPerSecond")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("WindGustString")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<decimal?>("WindSpeedMeterPerSecond")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("WindSpeedString")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.ToTable("Weather");
                });

            modelBuilder.Entity("SailScores.Database.Entities.WeatherSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal?>("Latitude")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal?>("Longitude")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("TemperatureUnits")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WindSpeedUnits")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WeatherSettings");
                });

            modelBuilder.Entity("SailScores.Database.Entities.BoatClass", b =>
                {
                    b.HasOne("SailScores.Database.Entities.Club", null)
                        .WithMany("BoatClasses")
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SailScores.Database.Entities.Club", b =>
                {
                    b.HasOne("SailScores.Database.Entities.ScoringSystem", "DefaultScoringSystem")
                        .WithMany("DefaultForClubs")
                        .HasForeignKey("DefaultScoringSystemId");

                    b.HasOne("SailScores.Database.Entities.WeatherSettings", "WeatherSettings")
                        .WithMany()
                        .HasForeignKey("WeatherSettingsId");
                });

            modelBuilder.Entity("SailScores.Database.Entities.Competitor", b =>
                {
                    b.HasOne("SailScores.Database.Entities.BoatClass", "BoatClass")
                        .WithMany()
                        .HasForeignKey("BoatClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SailScores.Database.Entities.Club", null)
                        .WithMany("Competitors")
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SailScores.Database.Entities.CompetitorFleet", b =>
                {
                    b.HasOne("SailScores.Database.Entities.Competitor", "Competitor")
                        .WithMany("CompetitorFleets")
                        .HasForeignKey("CompetitorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SailScores.Database.Entities.Fleet", "Fleet")
                        .WithMany("CompetitorFleets")
                        .HasForeignKey("FleetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("SailScores.Database.Entities.Fleet", b =>
                {
                    b.HasOne("SailScores.Database.Entities.Club", null)
                        .WithMany("Fleets")
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SailScores.Database.Entities.FleetBoatClass", b =>
                {
                    b.HasOne("SailScores.Database.Entities.BoatClass", "BoatClass")
                        .WithMany()
                        .HasForeignKey("BoatClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SailScores.Database.Entities.Fleet", "Fleet")
                        .WithMany("FleetBoatClasses")
                        .HasForeignKey("FleetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SailScores.Database.Entities.HistoricalResults", b =>
                {
                    b.HasOne("SailScores.Database.Entities.Series", "Series")
                        .WithMany()
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SailScores.Database.Entities.Race", b =>
                {
                    b.HasOne("SailScores.Database.Entities.Club", null)
                        .WithMany("Races")
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SailScores.Database.Entities.Fleet", "Fleet")
                        .WithMany()
                        .HasForeignKey("FleetId");

                    b.HasOne("SailScores.Database.Entities.Weather", "Weather")
                        .WithMany()
                        .HasForeignKey("WeatherId");
                });

            modelBuilder.Entity("SailScores.Database.Entities.Regatta", b =>
                {
                    b.HasOne("SailScores.Database.Entities.Club", null)
                        .WithMany("Regattas")
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SailScores.Database.Entities.ScoringSystem", "ScoringSystem")
                        .WithMany()
                        .HasForeignKey("ScoringSystemId");

                    b.HasOne("SailScores.Database.Entities.Season", "Season")
                        .WithMany()
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("SailScores.Database.Entities.RegattaFleet", b =>
                {
                    b.HasOne("SailScores.Database.Entities.Fleet", "Fleet")
                        .WithMany()
                        .HasForeignKey("FleetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SailScores.Database.Entities.Regatta", "Regatta")
                        .WithMany("RegattaFleet")
                        .HasForeignKey("RegattaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("SailScores.Database.Entities.RegattaSeries", b =>
                {
                    b.HasOne("SailScores.Database.Entities.Regatta", "Regatta")
                        .WithMany("RegattaSeries")
                        .HasForeignKey("RegattaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SailScores.Database.Entities.Series", "Series")
                        .WithMany()
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SailScores.Database.Entities.Score", b =>
                {
                    b.HasOne("SailScores.Database.Entities.Competitor", "Competitor")
                        .WithMany("Scores")
                        .HasForeignKey("CompetitorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SailScores.Database.Entities.Race", "Race")
                        .WithMany("Scores")
                        .HasForeignKey("RaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SailScores.Database.Entities.ScoreCode", b =>
                {
                    b.HasOne("SailScores.Database.Entities.ScoringSystem", null)
                        .WithMany("ScoreCodes")
                        .HasForeignKey("ScoringSystemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SailScores.Database.Entities.ScoringSystem", b =>
                {
                    b.HasOne("SailScores.Database.Entities.Club", null)
                        .WithMany("ScoringSystems")
                        .HasForeignKey("ClubId");

                    b.HasOne("SailScores.Database.Entities.ScoringSystem", "ParentSystem")
                        .WithMany()
                        .HasForeignKey("ParentSystemId");
                });

            modelBuilder.Entity("SailScores.Database.Entities.Season", b =>
                {
                    b.HasOne("SailScores.Database.Entities.Club", null)
                        .WithMany("Seasons")
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SailScores.Database.Entities.Series", b =>
                {
                    b.HasOne("SailScores.Database.Entities.Club", null)
                        .WithMany("Series")
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SailScores.Database.Entities.ScoringSystem", "ScoringSystem")
                        .WithMany()
                        .HasForeignKey("ScoringSystemId");

                    b.HasOne("SailScores.Database.Entities.Season", "Season")
                        .WithMany("Series")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("SailScores.Database.Entities.SeriesChartResults", b =>
                {
                    b.HasOne("SailScores.Database.Entities.Series", "Series")
                        .WithMany()
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SailScores.Database.Entities.SeriesRace", b =>
                {
                    b.HasOne("SailScores.Database.Entities.Race", "Race")
                        .WithMany("SeriesRaces")
                        .HasForeignKey("RaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SailScores.Database.Entities.Series", "Series")
                        .WithMany("RaceSeries")
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

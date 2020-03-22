﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace SailScores.Web
{
    public class Program
    {

        private static int? _sslPort = null;
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .Build();

        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
            try
            {
                _sslPort = GetSslPort();
                Log.Information("Starting web host");
                CreateWebHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Host terminated unexpectely");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static int? GetSslPort()
        {
            int retVal;
            if (Int32.TryParse(Configuration["sslPort"], out retVal))
            {
                return retVal;
            }
            return null;
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            if (_sslPort.HasValue)
            {

                return WebHost.CreateDefaultBuilder(args)
                    .ConfigureLogging(c =>
                        c.AddConsole())
                    .UseSetting("https_port", _sslPort.ToString())
                    .UseStartup<Startup>();
                    //.UseSerilog();
            }
            return WebHost.CreateDefaultBuilder(args)
                                    .ConfigureLogging(c =>
                        c.AddConsole())
                .UseStartup<Startup>();
                //.UseSerilog();
        }
    }
}

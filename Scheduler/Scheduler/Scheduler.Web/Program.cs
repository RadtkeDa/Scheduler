﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Scheduler.Infrastructure.Common.Extensions;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System;
using System.IO;
using System.Linq;

namespace Scheduler.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConfigureLogger();

            bool overrideMigration = false, overrideSeed = false;

            // Process command-line switches
            if (args.Contains("--migrate")) overrideMigration = true;
            if (args.Contains("--seed")) overrideSeed = true;

            if (overrideSeed || overrideMigration)
            {
                Log.Information("Starting seeding/migration process");
                BuildWebApplication(args)
                    .RunMigrations(overrideMigration) // Apply any new EF migrations
                    .RunSeeders(overrideSeed); // Run any auto-registered seeders

                return;
            }

            try
            {
                Log.Information("Starting web host");
                BuildWebApplication(args)
                    .RunMigrations()  // Apply any new EF migrations (requires appsetting)
                    .RunSeeders()     // Run any auto-registered seeders (classes that implement ISeeder) (requires appsetting)
                    .Run();           // Start the web host
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        /// <summary>
        /// Set up Serilog static logger
        /// </summary>
        public static void ConfigureLogger()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (String.IsNullOrEmpty(environment))
            {
                environment = "Production";
            }

            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddJsonFile($"appsettings.{environment}.json", optional: false)
                .Build();

            var sinkOpts = new MSSqlServerSinkOptions { TableName = "ServiceLog", AutoCreateSqlTable = false };
            var columnOpts = new ColumnOptions();

            var connSection = configuration.GetSection("ConnectionStrings");
            var connString = configuration.GetConnectionString("AppContext");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Debug()
                .WriteTo.MSSqlServer(
                    connString,
                    sinkOpts,
                    columnOptions: columnOpts)
                .CreateLogger();
        }

        public static WebApplication BuildWebApplication(string[] args)
        {
            var builder = WebApplication.CreateBuilder();

            builder.Configuration.AddEnvironmentVariables();
            builder.Host.UseSerilog(Log.Logger);

            var startup = new Startup(builder.Configuration);
            startup.ConfigureServices(builder.Services);

            var app = builder.Build();
            startup.Configure(app);

            return app;
        }

    }

}

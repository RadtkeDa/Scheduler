﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoverCore.Abstractions.Settings;
using Scheduler.Domain.Entities.Settings;
using Scheduler.Infrastructure.Common.Settings.Services;

namespace Scheduler.Infrastructure.Common.Settings
{
    public static class Startup
    {
        /// <summary>
        ///     Adds a singleton service of the typed ApplicationSettings stored in appsettings.json
        /// </summary>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Add ApplicationsSettings service
            var settings = configuration.GetSection("Settings").Get<ApplicationSettings>();

            if (settings == null)
            {
                throw new Exception("Missing Application Settings in configuration file");
            }

            settings.Email ??= new EmailSettings();

            services.AddSingleton(settings);

            services.AddScoped<ISettingsService<ApplicationSettings>, SettingsService<ApplicationSettings>>();

            // Adds IOptions capabilities
            services.AddOptions();
        }

        public static void Configure(IApplicationBuilder app, IConfiguration configuration)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var settingsService = serviceScope.ServiceProvider.GetRequiredService<ISettingsService<ApplicationSettings>>();

                // Load persisted settings
                settingsService.LoadPersistedSettings().GetAwaiter().GetResult();
            }
        }
    }

}

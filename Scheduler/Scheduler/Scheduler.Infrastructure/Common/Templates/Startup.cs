﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoverCore.Abstractions.Templates;
using Scheduler.Domain.Entities.Settings;
using Scheduler.Infrastructure.Common.Settings.Services;
using Scheduler.Infrastructure.Common.Templates.Services;

namespace Scheduler.Infrastructure.Common.Templates
{
    public static class Startup
    {
        /// <summary>
        ///     Adds a singleton service of the typed ApplicationSettings stored in appsettings.json
        /// </summary>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITemplateService, TemplateService>();
        }

        public static void Configure(IApplicationBuilder app, IConfiguration configuration)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                // Load templates
                var templateService = serviceScope.ServiceProvider.GetRequiredService<ITemplateService>();
            }
        }
    }

}

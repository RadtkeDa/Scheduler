﻿using Hangfire.Dashboard;

namespace Scheduler.Infrastructure.Common.Hangfire.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            var claims = httpContext?.User?.Identity;

            if (claims == null)
                return false;

            // Limit hangfire dashboard to just members with admin role
            return httpContext!.User!.Identity!.IsAuthenticated && httpContext.User.IsInRole("Admin");
        }
    }
}

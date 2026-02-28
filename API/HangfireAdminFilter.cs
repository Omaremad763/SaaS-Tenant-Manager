namespace SaaS_Tenant_Manager;

using Hangfire.Dashboard;

public class HangfireAdminFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        var env = httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();

        if (env.IsDevelopment())
            return true;

        if (!httpContext.User.Identity?.IsAuthenticated ?? false)
            return false;

        return httpContext.User.IsInRole("SystemAdmin");
    }
}

using System.Security.Claims;

using Application.Contracts;
namespace SaaS_Tenant_Manager.Middlewares;
public class TenantSecurityMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, ISaasServices services)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userTenantId = context.User.FindFirstValue("TenantId");
            var isSystemAdmin = context.User.IsInRole("SystemAdmin");
            if (isSystemAdmin)
            {
                await _next(context);
                return;
            }

            var requestdomain = context.Request.RouteValues["tenantSlug"]?.ToString();
            if (!string.IsNullOrEmpty(requestdomain))
            {
                var tenant = await services.TenantService.GetTenantByDomainAsync(requestdomain);

                if (tenant == null || tenant.Id.ToString() != userTenantId)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Unauthorized: You do not belong to this Tenant.");
                    return;
                }
            }
        }

        await _next(context);
    }
}
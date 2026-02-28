using System.Security.Claims;

using Application.Contracts;

public class FeatureAccessMiddleware(RequestDelegate _next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var pathSegments = context.Request.Path.Value?.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (pathSegments != null && pathSegments.Length >= 3 && pathSegments[1].Equals("features", StringComparison.OrdinalIgnoreCase))
        {
            var featureName = pathSegments[2];
            var tenantIdClaim = context.User.FindFirstValue("TenantId");

            if (Guid.TryParse(tenantIdClaim, out Guid id))
            {
                var services = context.RequestServices.GetRequiredService<ISaasServices>();

                var ct = context.RequestAborted;

                var access = await services.TenantFeatureService.CheckTenantAccesedFeatures(id, featureName, ct);

                if (access == null || !access.IsEnabled)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        error = $"this {featureName} isn't in your plan"
                    });
                    return;
                }
            }
        }

        await _next(context);
    }
}
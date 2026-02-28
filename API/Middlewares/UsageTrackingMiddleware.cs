namespace SaaS_Tenant_Manager.Middlewares;

using System.Net;
using System.Security.Claims;

using Application.Contracts;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

public class UsageTrackingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;

    public UsageTrackingMiddleware(RequestDelegate next, IMemoryCache cache)
    {
        _next = next;
        _cache = cache;
    }

    public async Task InvokeAsync(HttpContext context, ISaasServices services, CancellationToken cancellationToken)
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
            var planInfo = await services.TenantSubscriptionService.GetTenantSubscriptionAsync(Guid.Parse(userTenantId), cancellationToken);
            if (planInfo != null)
            {
                var cacheKey = $"RateLimit_{userTenantId}";
                var requestCount = _cache.Get<int?>(cacheKey) ?? 0;

                if (requestCount >= planInfo.MonthlyRequestLimit)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        error = "Rate limit exceeded. Upgrade your plan for more capacity."
                    });
                    return;
                }
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
                _cache.Set(cacheKey, requestCount + 1, cacheOptions);
            }
            await _next(context);
        }
    }
}
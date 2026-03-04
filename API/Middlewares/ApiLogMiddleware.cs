using System.Diagnostics;

using Application.Contracts.IService;

using Domain.Entities;
using Domain.Entities.MasterDB;

using Hangfire;
namespace SaaS_Tenant_Manager.Middlewares;
public class ApiLogMiddleware(RequestDelegate next, IBackgroundJobClient backgroundJobClient)
{
    private readonly RequestDelegate _next = next;
    private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;

    public async Task InvokeAsync(HttpContext context)
    {
        Stopwatch watch = Stopwatch.StartNew();
        var path = context.Request.Path.Value?.ToLower();

        if (string.IsNullOrEmpty(path) || !path.StartsWith("/api"))
        {
            await _next(context);
            return;
        }

        try
        {
            await _next(context);     
        }
        catch (Exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            throw;         
        }
        finally
        {
            watch.Stop();

            Guid? tenantId = null;
            if (context.Request.Query.TryGetValue("TenantId", out var rawId)  && Guid.TryParse(rawId, out var parsedId))
            {
                tenantId = parsedId;
            }

            var log = new ApiLog
            {
                TenantId = tenantId,
                Endpoint = context.Request.Path,
                HttpMethod = context.Request.Method,
                StatusCode = context.Response.StatusCode,         
                DurationMs = (long)watch.Elapsed.TotalMilliseconds,
                IpAddress = context.Connection.RemoteIpAddress?.ToString(),
                CreatedAt = DateTime.UtcNow
            };

            _backgroundJobClient.Enqueue<IApiLogService>(service => service.SaveLogAsync(log));
        }
    }
}
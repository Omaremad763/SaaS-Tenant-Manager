using Application.Contracts.IRepo;
using Application.DTOs;

using Domain.Entities.MasterDB;

using Infra.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;

namespace Infra.Contracts_Implementation.Repo;

public class APIlogRepo(MasterDbContext _context) : IAPIlogRepo
{
    public async Task<SystemMetricsDto> GetSystemMetricsAsync()
    {
        var last24Hours = DateTime.UtcNow.AddDays(-1);

        var logsQuery = _context.ApiLogs.AsNoTracking();

        var totalRequests = await logsQuery.CountAsync();
        var globalErrorCount = await logsQuery.CountAsync(l => l.StatusCode >= 400);
        var avgResponseTime = await logsQuery.AverageAsync(l => (double?)l.DurationMs) ?? 0;

        var topTenants = await logsQuery
            .GroupBy(l => l.TenantId)
            .Select(g => new
            {
                Id = g.Key,
                Count = g.Count(),
                Name = g.Max(l => l.Tenant.Name)
            })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .Select(x => new TenantUsageDto(x.Name ?? "System/None", x.Count))
            .ToListAsync();

        var trafficStats = await _context.ApiLogs
            .Where(a => a.CreatedAt >= last24Hours)
            .GroupBy(a => a.CreatedAt.Hour)
            .Select(g => new
            {
                Hour = g.Key,
                Total = g.Count(),
                Errors = g.Count(e => e.StatusCode >= 400)
            })
            .OrderBy(x => x.Hour)
            .Select(x => new HourlyTrafficDto(
                x.Hour,
                x.Total,
                x.Errors
            ))
            .ToListAsync();

        return new SystemMetricsDto(
            totalRequests,
            globalErrorCount,
            Math.Round(avgResponseTime, 2),
            topTenants,
            trafficStats
        );
    }

    public async Task SaveLogAsync(ApiLog log)
    {
        _context.ApiLogs.Add(log);
        await _context.SaveChangesAsync();
    }
}
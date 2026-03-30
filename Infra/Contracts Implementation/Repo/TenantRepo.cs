using Application.Contracts.IRepo;
using Application.DTOs;

using Domain.Entities.MasterDB;

using Infra.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;

namespace Infra.Contracts_Implementation.Repo;

public class TenantRepo(MasterDbContext _context) : ITenantRepo
{
    public async Task<Tenant?> GetTenantBySlugAndDomainAsync(string slug, string domain) => await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == slug || t.TenantDomain == domain);

    public async Task<Tenant?> GetTenantByDomainAsync(string domain) => await _context.Tenants.FirstOrDefaultAsync(t => t.TenantDomain == domain);

    public async Task AddTenantAsync(Tenant tenant) => await _context.Tenants.AddAsync(tenant);

    public async Task<List<TenantManagementDto>> GetAllTenantsManagementAsync()
    {
        var TenantData =
         await _context.Tenants
            .Select(t => new TenantManagementDto(
                t.Name,
                t.TenantSubscriptionTable.SubscriptionPlanTable.PlanName,
                _context.Features.Select(f => new FeatureStatusDto(
                    f.Id,
                    f.FeatureName,
                    t.TenantSubscriptionTable.SubscriptionPlanTable.PlanFeatures.Any(pf => pf.FeatureId == f.Id),
                    t.TenantFeatures.Any(tf => tf.FeatureId == f.Id && tf.IsEnabled)
                )).ToList()
            )).ToListAsync();
        return TenantData;
    }
}
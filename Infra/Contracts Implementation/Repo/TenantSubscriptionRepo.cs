using Application.Contracts.IRepo;

using Domain.Entities.MasterDB;

using Infra.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;

namespace Infra.Contracts_Implementation.Repo;

public class TenantSubscriptionRepo(MasterDbContext context) : ITenantSubscriptionRepo
{
    public async Task AddTenantSubscriptionAsync(TenantSubscription subscription)
    {
        await context.TenantSubscriptions.AddAsync(subscription);
    }

    public async Task<TenantSubscription?> GetTenantSubscriptionAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        return await context.TenantSubscriptions
            .Include(s => s.SubscriptionPlanTable)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.TenantId == tenantId && s.IsActive, cancellationToken);
    }
}
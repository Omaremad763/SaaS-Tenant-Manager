using Application.Contracts.IRepo;

using Domain.Entities.MasterDB;

using Infra.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;

namespace Infra.Contracts_Implementation.Repo;

public class PlanFeatureRepo(MasterDbContext context) : IPlanFeatureRepo
{
    public async Task<IEnumerable<PlanFeature>> GetFeaturesByPlanIdAsync(Guid planId)
    {
        return await context.PlanFeatures.Where(pf => pf.SubscriptionPlanId == planId).ToListAsync();
    }
}
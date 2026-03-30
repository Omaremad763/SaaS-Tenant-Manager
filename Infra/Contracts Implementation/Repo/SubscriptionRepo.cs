using Application.Contracts.IRepo;

using Domain.Entities.MasterDB;

using Infra.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;

namespace Infra.Contracts_Implementation.Repo;

public class SubscriptionRepo(MasterDbContext context) : ISubscriptionRepo
{
    public async Task<SubscriptionPlan?> GetPlanByIdAsync(int planName)
    {
        return await context.SubscriptionPlans.FirstOrDefaultAsync(p => p.PlanTier == planName);
    }
}
using Application.Contracts.IRepo;

using Domain.Entities.MasterDB;

using Infra.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;

namespace TenantFeatureRepoImp;

public class TenantFeatureRepo(MasterDbContext context) : ITenantFeatureRepo
{
    public async Task AddRangeTenantFeaturAsync(IEnumerable<TenantFeature> tenantFeatures)
    {
        await context.TenantFeatures.AddRangeAsync(tenantFeatures);
    }

    public async Task<TenantFeature?> GetAccessedFeatures(Guid tenantId, string featureName)
    {
        return await context.TenantFeatures
                .Include(tf => tf.FeatureTable)
                .FirstOrDefaultAsync(tf =>
                    tf.TenantId == tenantId &&
                    tf.FeatureTable.FeatureName == featureName);
    }

    public async Task<bool> AddTenantFeatureAsync(TenantFeature tenantFeature)
    {
        var add = await context.TenantFeatures.AddAsync(tenantFeature);
        if (add.State != EntityState.Added)
        {
            return false;
        }
        return true;
    }
}
using Domain.Entities.MasterDB;

namespace Application.Contracts.IRepo;
public interface ITenantFeatureRepo
{
    Task AddRangeTenantFeaturAsync(IEnumerable<TenantFeature> tenantFeatures);
    Task<TenantFeature?> GetAccessedFeatures(Guid tenantId, string featureName);
    Task<bool> AddTenantFeatureAsync(TenantFeature tenantFeature);
}

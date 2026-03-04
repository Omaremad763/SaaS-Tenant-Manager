using Application.Contracts.IRepo;

namespace Application.Contracts;
public interface IUnitofWork:IDisposable
{
    IUserRepository UserRepository { get; }
    ITenantFeatureRepo TenantFeatureRepo { get; }

    public IPlanFeatureRepo PlanFeatureRepo { get; }
    public ITenantSubscriptionRepo TenantSubscriptionRepo { get; }
    public ITenantRepo TenantRepo { get; }
    public IFeatureRepo FeatureRepo { get; }
    public ISubscriptionRepo SubscriptionRepo { get; }
    Task<int> CommitAsync();
}


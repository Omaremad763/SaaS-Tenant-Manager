using Application.Contracts.IRepo;

using Microsoft.EntityFrameworkCore.Storage;

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
    public IAPIlogRepo APIlogRepo { get; }
    public IShipmentRepo shipmentRepo { get; }
    public IClientRepository ClientRepository { get; }
    Task<int> CommitAsync();
    Task<int> TenantCommitAsync();

    Task<IDbContextTransaction> BeginTransactionAsync();

}


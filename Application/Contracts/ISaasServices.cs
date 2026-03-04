using Application.Contracts.IService;

namespace Application.Contracts
{
    public interface ISaasServices
    {
        public IDbMigrationService DbMigrationService { get; }
        public IUserService UserService { get; }
        public IPlanFeatureService PlanFeatureService { get; }
        public ITenantFeatureService TenantFeatureService { get; }
        public ITenantSubscriptionService TenantSubscriptionService { get; }
        public ITenantService TenantService { get;}
        public IFeatureService FeatureService {  get; }

    }
}
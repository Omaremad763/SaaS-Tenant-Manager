using Application.Contracts;
using Application.Contracts.IRepo;

using Domain.Entities.MasterDB;

using Infra.Contracts_Implementation.Auth_Page;
using Infra.Contracts_Implementation.Repo;
using Infra.Persistence.Contexts;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

using TenantFeatureRepoImp;

namespace Infrastructure.Contracts_Implementation
{
    public class UnitofWork(MasterDbContext context, UserManager<User> _userManager,RoleManager<UserRoles> roleManager) 
        : IUnitofWork
    {
        public IUserRepository UserRepository =>  new UserRepository( _userManager, roleManager, context);
        public ITenantFeatureRepo TenantFeatureRepo =>  new TenantFeatureRepo(context);
        public IPlanFeatureRepo PlanFeatureRepo =>  new PlanFeatureRepo(context);
        public ITenantSubscriptionRepo TenantSubscriptionRepo =>  new TenantSubscriptionRepo(context);
        public ITenantRepo TenantRepo =>  new TenantRepo(context);
        public ISubscriptionRepo SubscriptionRepo =>  new SubscriptionRepo(context);

        public IFeatureRepo FeatureRepo => new FeatureRepo(context);

        public async Task<int> CommitAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();GC.SuppressFinalize(this);
        }
    }
}

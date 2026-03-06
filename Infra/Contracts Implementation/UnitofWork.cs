using Application.Contracts;
using Application.Contracts.IRepo;

using Domain.Entities.MasterDB;

using IdGen;

using Infra.Contracts_Implementation.Auth_Page;
using Infra.Contracts_Implementation.Repo;
using Infra.Persistence.Contexts;
using Infra.TenantDBRepo;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

using TenantFeatureRepoImp;

namespace Infrastructure.Contracts_Implementation
{
    public class UnitofWork(MasterDbContext Mastercontext, 
        UserManager<User> 
        _userManager,
        RoleManager<UserRoles> roleManager,
        TenantDbContext tenantDb,
        IIdGenerator<long> _idGenerator
        )
        : IUnitofWork
    {
        public IUserRepository UserRepository =>  new UserRepository( _userManager, roleManager, Mastercontext);
        public ITenantFeatureRepo TenantFeatureRepo =>  new TenantFeatureRepo(Mastercontext);
        public IPlanFeatureRepo PlanFeatureRepo =>  new PlanFeatureRepo(Mastercontext);
        public ITenantSubscriptionRepo TenantSubscriptionRepo =>  new TenantSubscriptionRepo(Mastercontext);
        public ITenantRepo TenantRepo =>  new TenantRepo(Mastercontext);
        public ISubscriptionRepo SubscriptionRepo =>  new SubscriptionRepo(Mastercontext);

        public IFeatureRepo FeatureRepo => new FeatureRepo(Mastercontext);
        public IAPIlogRepo APIlogRepo => new APIlogRepo(Mastercontext);
        public IShipmentRepo shipmentRepo=> new ShipmentRepo(tenantDb, _idGenerator);
        public IClientRepository ClientRepository => new ClientRepository(tenantDb);
        public async Task<int> CommitAsync()
        {
            return await Mastercontext.SaveChangesAsync();
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await Mastercontext.Database.BeginTransactionAsync();
        }
        public void Dispose()
        {
            Mastercontext.Dispose();GC.SuppressFinalize(this);
        }
        public async Task<int> TenantCommitAsync()
        {
            return await tenantDb.SaveChangesAsync();
        }
    }
}

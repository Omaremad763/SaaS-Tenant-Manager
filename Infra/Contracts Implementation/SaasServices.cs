using ApilogsServiceImp;

using Application.Contracts;
using Application.Contracts.IService;

using AutoMapper;

using Infra.Contracts_Implementation.Entites_Contracts.TenantFeatures;
using Infra.Contracts_Implementation.RegistrionPage;
using Infra.Contracts_Implementation.Repo;
using Infra.Contracts_Implementation.Service;
using Infra.Persistence.Contexts;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using User_service_Imp;

namespace Infrastructure.Contracts_Implementation;

    public class SaasServices(IMapper mapper,IUnitofWork unitofWork,IServiceProvider provider
        ,IMediator mediator
        ) 
    : ISaasServices
{
    public IDbMigrationService DbMigrationService => new DbMigrationService(provider);
    public IUserService UserService => new UserService(mapper,mediator, unitofWork);
    public IPlanFeatureService PlanFeatureService => new PlanFeatureService( unitofWork);
    public ITenantFeatureService TenantFeatureService => new TenantFeatureService( unitofWork);
    public ITenantSubscriptionService TenantSubscriptionService => new TenantSubscriptionService(unitofWork,mapper);
    public ITenantService TenantService => new TenantService(unitofWork);
    public IApiLogService ApiLogService =>  new ApiLogService( unitofWork);
    public IFeatureService FeatureService => new FeatureServcie(unitofWork);
    public IShipmentService ShipmentService => new ShipmentService(unitofWork);
    public IClientService ClientService => new ClientService(unitofWork,mapper);
}


using Application.Contracts;
using Application.Contracts.IService;

using AutoMapper;

using Infra.Contracts_Implementation.Entites_Contracts.TenantFeatures;
using Infra.Contracts_Implementation.RegistrionPage;
using Infra.Contracts_Implementation.Service;

using MediatR;

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
    public IFeatureService FeatureServcie => new FeatureServcie(unitofWork);
    public IFeatureService FeatureService =>  new FeatureServcie(unitofWork);
}


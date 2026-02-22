using Application.Contracts;
using Application.Contracts.Auth;

using AutoMapper;

using Infra.Contracts_Implementation.RegistrionPage;

using MediatR;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Contracts_Implementation;

    public class SaasServices(IMapper mapper,IUnitofWork unitofWork,IServiceProvider provider,IMediator mediator,IConfiguration config) : ISaasServices
{
    public IDbMigrationService DbMigrationService => new DbMigrationService(provider);
    public IUserService UserService => new UserService(mapper,mediator, unitofWork);
}


using Application;
using Application.Contracts;

using Domain.Entities;

using FluentValidation;

using Infra.Persistence;

using Infrastructure.Contracts_Implementation;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace Infra.Extentions;

public static class DependenciesCollector
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        var assembly = typeof(IApplicationHandlerMarker).Assembly;
        var DatabaseConfig = Environment.GetEnvironmentVariable("SaasDatabaseConfig");
        services.AddDbContext<ApplicationDbContext>
         (options =>
         {
             options.UseNpgsql(DatabaseConfig);
         });
        services.AddScoped<ISaasServices, SaasServices>();
        services.AddScoped<IUnitofWork, UnitofWork>();
        services.AddAutoMapper(cfg => {
            cfg.AddProfile<AutoMapperProfile>();
        }, typeof(AutoMapperProfile).Assembly);
        #region Mediator
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddValidatorsFromAssembly(assembly);
        #endregion
        services.AddIdentity<User, UserRoles>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }
}
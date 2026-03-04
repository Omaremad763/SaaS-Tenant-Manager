using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using ApilogsServiceImp;

using Application;
using Application.Contracts;
using Application.Contracts.IService;

using Domain.Entities.MasterDB;

using FluentValidation;

using Hangfire;
using Hangfire.PostgreSql;

using Infra.Persistence.Contexts;

using Infrastructure.Contracts_Implementation;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
namespace Infra.Extentions;

public static class DependenciesCollector
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        var issuer = Environment.GetEnvironmentVariable("SaasJWTIssuer");
        var audience = Environment.GetEnvironmentVariable("SaasJWTAudience");
        var jwtKey = Environment.GetEnvironmentVariable("SaasJwtKey");
        var assembly = typeof(IApplicationHandlerMarker).Assembly;
        var DatabaseConfig = Environment.GetEnvironmentVariable("SaasDatabaseConfig");
        services.AddDbContext<MasterDbContext>
         (options =>
         {
             options.UseNpgsql(DatabaseConfig);
         });
        services.AddScoped<ISaasServices, SaasServices>();
        services.AddScoped<IApiLogService, ApiLogService>();
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
        .AddEntityFrameworkStores<MasterDbContext>()
        .AddDefaultTokenProviders();
        #region Hangifre

        services.AddHangfire(config => config
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UsePostgreSqlStorage(options =>
        {
            options.UseNpgsqlConnection(DatabaseConfig);
        }));
            services.AddHangfireServer();
        #endregion

        #region Auth
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
                    .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,        
                    ValidAudience = audience,             
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                    NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
                };
            });
        services.AddAuthorization(); 
        #endregion

        return services;
    }
}


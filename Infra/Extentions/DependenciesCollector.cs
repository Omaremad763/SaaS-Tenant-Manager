using Application;

using FluentValidation;

using Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure.Extentions;
public static class DependenciesCollector
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        var assembly = typeof(IApplicationHandlerMarker).Assembly;
        var DatabaseConfig = Environment.GetEnvironmentVariable("SaasDatabaseConfig");
        services.AddDbContext<ApplicationDbContext>
         (options =>
         {
             options.UseNpgsql((DatabaseConfig));
         });
        //services.AddScoped<IAI_AnalyticsServices, AI_AnalyticsServices>();
        //services.AddHttpClient<IAI_InsightService, Ai_InsightService>();
        //services.AddScoped<IUnitofWork, UnitofWork>();
        //services.AddScoped<IData_InegstionService, Data_InegstionService>();
        #region Mediator
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddValidatorsFromAssembly(assembly);
        #endregion
        return services;
    }
}
using Application.Contracts;

using Hangfire;

using Infra.Extentions;
using Infra.Persistence.Contexts;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

using SaaS_Tenant_Manager;
using SaaS_Tenant_Manager.Middlewares;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddServices(builder.Configuration);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("VercelPolicy", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
        {
            return string.IsNullOrEmpty(origin) ||
                   origin.EndsWith(".vercel.app") ||
                   origin.Contains("localhost");
        })
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var Mastercontext = services.GetRequiredService<MasterDbContext>();
        logger.LogInformation("\x1b[36m[System] Starting Master Database Migrations...\x1b[0m");

        await Mastercontext.Database.MigrateAsync();

        logger.LogInformation("\x1b[32m[System] Master Database is up-to-date.\x1b[0m");

        var saasServices = services.GetRequiredService<ISaasServices>();
        logger.LogInformation("\x1b[34m[System] Ensuring System Admin User exists...\x1b[0m");

        await saasServices.UserService.EnsureSystemAdminAsync();

        logger.LogInformation("\x1b[32m[System] System Admin check passed successfully.\x1b[0m");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "\x1b[31m[Critical] Startup initialization failed!\x1b[0m");
        throw;
    }
}


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseCors(policyName: "VercelPolicy");
    app.UseHttpsRedirection();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<TenantSecurityMiddleware>();
app.UseMiddleware<FeatureAccessMiddleware>();
app.UseMiddleware<ApiLogMiddleware>();
app.UseHangfireDashboard("/HangfireSass", new DashboardOptions
{
    Authorization = [new HangfireAdminFilter()],
    DashboardTitle = "SaaS Manager - Background Jobs"
});
app.MapControllers();
await app.RunAsync();

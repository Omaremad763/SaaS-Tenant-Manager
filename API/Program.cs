using Application.Contracts;

using Hangfire;

using Infra.Extentions;

using Microsoft.AspNetCore.Builder;

using SaaS_Tenant_Manager;
using SaaS_Tenant_Manager.Middlewares;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddServices();
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
    var saasServices = scope.ServiceProvider.GetRequiredService<ISaasServices>();
    var created = await saasServices.UserService.EnsureSystemAdminAsync();
        Console.WriteLine(created);
}
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.UseCors("VercelPolicy");
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
app.Run();

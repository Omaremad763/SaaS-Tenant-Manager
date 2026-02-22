using Application.Contracts;

using Infra.Extentions;

using Middleware;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddServices();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
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
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<TenantSecurityMiddleware>();
app.UseCors("VercelPolicy");
app.UseHttpsRedirection(); 
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

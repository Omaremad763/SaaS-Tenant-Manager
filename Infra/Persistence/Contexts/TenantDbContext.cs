using Domain.Entities.TenantDBEntities;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infra.Persistence.Contexts;

public class TenantDbContext(
    DbContextOptions<TenantDbContext> options,
    IHttpContextAccessor httpContextAccessor,
    MasterDbContext masterDb) : DbContext(options)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly MasterDbContext _masterDb = masterDb;

    public TenantDbContext(DbContextOptions<TenantDbContext> options)
     : this(options, null!, null!)
    {
    }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<ShipmentItem> ShipmentItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            base.OnConfiguring(optionsBuilder);
            return;
        }
        //Database-per-Tenant Strategy filter
        var user = _httpContextAccessor?.HttpContext?.User;
        if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
        {
            var tenantIdClaim = user.Claims.FirstOrDefault(c => c.Type == "TenantId")?.Value;

            if (!string.IsNullOrWhiteSpace(tenantIdClaim) && _masterDb != null)
            {
                var connectionString = _masterDb.Tenants
                    .AsNoTracking()
                    .Where(t => t.Id.ToString() == tenantIdClaim)
                    .Select(t => t.ConnectionString)
                    .FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(connectionString))
                {
                    optionsBuilder.UseNpgsql(connectionString);
                    return;
                }
            }
            base.OnConfiguring(optionsBuilder);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Shipment>()
            .HasOne(s => s.Client)
            .WithMany(c => c.Shipments)
            .HasForeignKey(s => s.ClientId);

        modelBuilder.Entity<ShipmentItem>()

            .HasOne(si => si.Shipment)

            .WithMany(s => s.Items)

            .HasForeignKey(si => si.ShipmentId);
    }
}

public class TenantDbContextFactory : IDesignTimeDbContextFactory<TenantDbContext>
{
    public TenantDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json", optional: true)
         .AddEnvironmentVariables()
         .Build();
        var databaseConfig = configuration["DefaultConnection"];
        var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();
        optionsBuilder.UseNpgsql(databaseConfig);

        return new TenantDbContext(optionsBuilder.Options, null!, null!);
    }
}
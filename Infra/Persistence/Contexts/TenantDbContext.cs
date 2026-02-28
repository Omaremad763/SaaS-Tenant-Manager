using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Entities.TenantDB_Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infra.Persistence.Contexts;
public class TenantDbContext(DbContextOptions<TenantDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<ShipmentItem> ShipmentItems { get; set; }
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
        var databaseConfig = Environment.GetEnvironmentVariable("SaasDatabaseConfig");

        var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();
        optionsBuilder.UseNpgsql(databaseConfig);

        return new TenantDbContext(optionsBuilder.Options);
    }
}

using Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence;
public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
        builder.Property(t => t.Slug).IsRequired().HasMaxLength(50);
        builder.HasIndex(t => t.Slug).IsUnique();
        builder.HasIndex(t => t.TenantDomain).IsUnique();
        builder.Property(t => t.ConnectionString).IsRequired();
    }
}
public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
    {
        builder.HasKey(sp => sp.Id);
        builder.Property(sp => sp.PlanName).IsRequired().HasMaxLength(50);
        builder.Property(sp => sp.Price).HasColumnType("decimal(18,2)");
    }
}

public class ApiLogConfiguration : IEntityTypeConfiguration<ApiLog>
{
    public void Configure(EntityTypeBuilder<ApiLog> builder)
    {
        builder.HasKey(al => al.Id);
        builder.Property(al => al.Endpoint).IsRequired().HasMaxLength(500);
        builder.HasOne(al => al.Tenant)
               .WithMany(t => t.ApiLogs)
               .HasForeignKey(al => al.TenantId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(al => al.TenantId);
    }
}

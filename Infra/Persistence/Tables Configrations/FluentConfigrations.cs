using Domain.Entities.MasterDB;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasKey(keyExpression: ts => ts.Id);
        builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
        builder.Property(t => t.Slug).IsRequired().HasMaxLength(50);
        builder.HasIndex(t => t.Slug).IsUnique();
        builder.HasIndex(t => t.TenantDomain).IsUnique();
        builder.Property(t => t.ConnectionString).IsRequired();
        builder.HasMany(t => t.ApiLogs)
               .WithOne(a => a.Tenant)
               .HasForeignKey(a => a.TenantId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
    {
        builder.HasKey(keyExpression: ts => ts.Id);
        builder.Property(sp => sp.PlanName).IsRequired().HasMaxLength(50);
        builder.Property(sp => sp.Price).HasColumnType("decimal(18,2)");
    }
}

public class ApiLogConfiguration : IEntityTypeConfiguration<ApiLog>
{
    public void Configure(EntityTypeBuilder<ApiLog> builder)
    {
        builder.HasKey(keyExpression: ts => ts.Id);
        builder.Property(al => al.Endpoint).IsRequired().HasMaxLength(500);
        builder.HasOne(al => al.Tenant)
               .WithMany(t => t.ApiLogs)
               .HasForeignKey(al => al.TenantId)
               .OnDelete(DeleteBehavior.Cascade);
        builder.Property(l => l.IpAddress).HasMaxLength(50);
        builder.HasIndex(al => new { al.TenantId, al.CreatedAt });
    }
}

public class TenantSubscriptionConfiguration : IEntityTypeConfiguration<TenantSubscription>
{
    public void Configure(EntityTypeBuilder<TenantSubscription> builder)
    {
        builder.HasKey(pf => new { pf.TenantId, pf.SubscriptionPlanId });
        // One SubscriptionPlan -> Many TenantSubscriptions
        builder.HasOne(ts => ts.SubscriptionPlanTable)
               .WithMany(p => p.TenantSubscriptionTable)
               .HasForeignKey(ts => ts.SubscriptionPlanId)
               .OnDelete(DeleteBehavior.Restrict);

        // One Tenant -> One TenantSubscription
        builder.HasOne(ts => ts.TenantTable)
               .WithOne(t => t.TenantSubscriptionTable)
               .HasForeignKey<TenantSubscription>(ts => ts.TenantId)
               .OnDelete(DeleteBehavior.Cascade);
    }

    public class PlanFeatureConfiguration : IEntityTypeConfiguration<PlanFeature>
    {
        public void Configure(EntityTypeBuilder<PlanFeature> builder)
        {
            builder.HasKey(pf => new { pf.SubscriptionPlanId, pf.FeatureId });

            builder.HasOne(tf => tf.SubscriptionPlanTable)
                   .WithMany(t => t.PlanFeatures)
                   .HasForeignKey(tf => tf.SubscriptionPlanId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(tf => tf.FeatureTable)
                   .WithMany(f => f.PlanFeatureTable)
                   .HasForeignKey(tf => tf.FeatureId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class FeatureConfiguration : IEntityTypeConfiguration<TenantFeature>
    {
        public void Configure(EntityTypeBuilder<TenantFeature> builder)
        {
            builder.HasKey(pf => new { pf.TenantId, pf.FeatureId });
            builder.HasOne(tf => tf.TenantTable)
                   .WithMany(t => t.TenantFeatures)
                   .HasForeignKey(tf => tf.TenantId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(tf => tf.FeatureTable)
                   .WithMany(f => f.TenantFeatureTable)
                   .HasForeignKey(tf => tf.FeatureId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
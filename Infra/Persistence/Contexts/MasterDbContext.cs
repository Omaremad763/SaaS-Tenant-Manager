using Application;
using Application.Enums;

using Domain.Entities.MasterDB;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infra.Persistence.Contexts;

public class MasterDbContext(DbContextOptions<MasterDbContext> options) : IdentityDbContext<User, UserRoles, Guid>(options)
{
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<SubscriptionPlan> SubscriptionPlans => Set<SubscriptionPlan>();
    public DbSet<ApiLog> ApiLogs => Set<ApiLog>();
    public DbSet<Feature> Features => Set<Feature>();
    public DbSet<PlanFeature> PlanFeatures => Set<PlanFeature>();
    public DbSet<TenantSubscription> TenantSubscriptions => Set<TenantSubscription>();
    public DbSet<TenantFeature> TenantFeatures => Set<TenantFeature>();

    protected void ConfigureSubscriptionPlans(ModelBuilder modelBuilder)
    {
        var freePlanId = Guid.Parse("C3333333-3333-3333-3333-333333333333");
        var proPlanId = Guid.Parse("D4444444-4444-4444-4444-444444444444");
        var enterprisePlanId = Guid.Parse("E5555555-5555-5555-5555-555555555555");

        modelBuilder.Entity<SubscriptionPlan>().HasData(
            new SubscriptionPlan
            {
                Id = freePlanId,
                PlanName = "Free Start",
                PlanTier = (int)PlanTier.Free,
                Price = 0,
                MaxRequestsPerMinute = 100,
                MaxUsers = 1,
                StorageLimitGb = 5,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new SubscriptionPlan
            {
                Id = proPlanId,
                PlanName = "Pro Business",
                PlanTier = (int)PlanTier.Pro,
                Price = 50,
                MaxRequestsPerMinute = 500,
                MaxUsers = 10,
                StorageLimitGb = 20,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new SubscriptionPlan
            {
                Id = enterprisePlanId,
                PlanName = "Enterprise Business",
                PlanTier = (int)PlanTier.Enterprise,
                Price = 100,
                MaxRequestsPerMinute = 1000,
                MaxUsers = 20,
                StorageLimitGb = 50,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }

    protected void ConfigurePlanFeatures(ModelBuilder modelBuilder)
    {
        //feature IDS
        var aiFeatureId = Guid.Parse("A1111111-1111-1111-1111-111111111111");
        var reportsFeatureId = Guid.Parse("B2222222-2222-2222-2222-222222222222");
        var multiUserId = Guid.Parse("F3333333-3333-3333-3333-333333333333");

        //Plan IDS
        var freePlanId = Guid.Parse("C3333333-3333-3333-3333-333333333333");
        var proPlanId = Guid.Parse("D4444444-4444-4444-4444-444444444444");
        var enterprisePlanId = Guid.Parse("E5555555-5555-5555-5555-555555555555");

        #region FeatureTable

        modelBuilder.Entity<Feature>().HasData(
        new Feature
        {
            Id = aiFeatureId,
            FeatureName = "AI Insights",
            FeatureCode = FeatureCodes.AiInsights,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new Feature
        {
            Id = reportsFeatureId,
            FeatureName = "Advanced Reporting",
            FeatureCode = FeatureCodes.AdvancedReporting,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new Feature
        {
            Id = multiUserId,
            FeatureName = "MultiUser",
            FeatureCode = FeatureCodes.MultiUser,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        }
     );

        #endregion FeatureTable

        #region PlanFeatureTable

        modelBuilder.Entity<PlanFeature>().HasData(
        new PlanFeature
        {
            SubscriptionPlanId = freePlanId,
            FeatureId = aiFeatureId,
            IsEnabledForPlan = true
        },

            new PlanFeature
            {
                SubscriptionPlanId = proPlanId,
                FeatureId = aiFeatureId,
                IsEnabledForPlan = true
            },
            new PlanFeature
            {
                SubscriptionPlanId = proPlanId,
                FeatureId = reportsFeatureId,
                IsEnabledForPlan = true
            },
            new PlanFeature
            {
                SubscriptionPlanId = enterprisePlanId,
                FeatureId = aiFeatureId,
                IsEnabledForPlan = true
            },
            new PlanFeature
            {
                SubscriptionPlanId = enterprisePlanId,
                FeatureId = reportsFeatureId,
                IsEnabledForPlan = true
            },
            new PlanFeature
            {
                SubscriptionPlanId = enterprisePlanId,
                FeatureId = multiUserId,
                IsEnabledForPlan = true
            }
);

        #endregion PlanFeatureTable
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MasterDbContext).Assembly);


        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
        });

        modelBuilder.Entity<UserRoles>(entity =>
        {
            entity.ToTable("Roles");
        });
        ConfigureSubscriptionPlans(modelBuilder);
        ConfigurePlanFeatures(modelBuilder);
    }
}

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<MasterDbContext>
{
    public MasterDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json", optional: true)
         .AddEnvironmentVariables()
         .Build();
        var databaseConfig = configuration["DefaultConnection"];

        var optionsBuilder = new DbContextOptionsBuilder<MasterDbContext>();
        optionsBuilder.UseNpgsql(databaseConfig);

        return new MasterDbContext(optionsBuilder.Options);
    }
}
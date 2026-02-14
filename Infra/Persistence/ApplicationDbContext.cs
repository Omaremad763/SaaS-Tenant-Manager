using Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Tenant> Tenants => Set<Tenant>();
        public DbSet<SubscriptionPlan> SubscriptionPlans => Set<SubscriptionPlan>();
        public DbSet<ApiLog> ApiLogs => Set<ApiLog>();
        public DbSet<FeatureFlag> FeatureFlags => Set<FeatureFlag>();
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
    //factory pattern to create instance of ApplicationDbContext at design time for migrations
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var DatabaseConfig = Environment.GetEnvironmentVariable("SaasDatabaseConfig");
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(DatabaseConfig);
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }

}

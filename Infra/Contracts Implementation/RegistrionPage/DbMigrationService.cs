using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Contracts.Auth;

using Infra.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Contracts_Implementation.RegistrionPage;

    public class DbMigrationService : IDbMigrationService
    {
        private readonly IServiceProvider _serviceProvider;
        public DbMigrationService(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;
        public async Task MigrateTenantDatabaseAsync(string connectionString)
        {
            using var scope = _serviceProvider.CreateScope();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            using var context = new ApplicationDbContext(optionsBuilder.Options);

            await context.Database.MigrateAsync();
        }
    }


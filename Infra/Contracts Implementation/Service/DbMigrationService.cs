using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Contracts.IService;

using Infra.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Contracts_Implementation.RegistrionPage;

    public class DbMigrationService(IServiceProvider serviceProvider) : IDbMigrationService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task MigrateTenantDatabaseAsync(string connectionString)
        {
            using var scope = _serviceProvider.CreateScope();
            var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();
        optionsBuilder.UseNpgsql(connectionString, x =>
            x.MigrationsAssembly("Infra")    
             .MigrationsHistoryTable("__TenantMigrationHistory")     
        ); using var context = new TenantDbContext(optionsBuilder.Options);
            await context.Database.MigrateAsync();
        }
    }


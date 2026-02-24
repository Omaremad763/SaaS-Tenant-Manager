using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Auth
{
    public interface IDbMigrationService
    {
        Task MigrateTenantDatabaseAsync(string connectionString);
    }
}

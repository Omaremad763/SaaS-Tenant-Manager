using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Contracts.IRepo;

using Domain.Entities.MasterDB;

using Infra.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;

namespace Infra.Contracts_Implementation.Repo;
public class TenantRepo(MasterDbContext _context) : ITenantRepo
{
    public async Task<Tenant?> GetTenantBySlugAndDomainAsync(string slug, string domain) => await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == slug || t.TenantDomain == domain);
    public async Task<Tenant?> GetTenantByDomainAsync(string domain) => await _context.Tenants.FirstOrDefaultAsync(t => t.TenantDomain == domain);
    public async Task AddTenantAsync(Tenant tenant) => await _context.Tenants.AddAsync(tenant);
}

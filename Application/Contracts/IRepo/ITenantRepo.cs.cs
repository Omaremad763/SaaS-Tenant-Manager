using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.DTOs;

using Domain.Entities.MasterDB;

namespace Application.Contracts.IRepo;
public interface ITenantRepo
{
    Task<Tenant?> GetTenantBySlugAndDomainAsync(string slug, string domain);
    Task<Tenant?> GetTenantByDomainAsync(string domain);
    Task AddTenantAsync(Tenant tenant);
    Task<List<TenantManagementDto>> GetAllTenantsManagementAsync();
}

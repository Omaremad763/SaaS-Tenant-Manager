using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Contracts;
using Application.Contracts.IService;

using Domain.Entities.MasterDB;

using Microsoft.EntityFrameworkCore;

namespace Infra.Contracts_Implementation.Service;
public class TenantService(IUnitofWork unitofWork):ITenantService
{
    public async Task<Tenant?> GetTenantBySlugAndDomainAsync(string slug, string domain) => await unitofWork.TenantRepo.GetTenantBySlugAndDomainAsync(slug, domain);
    public async Task<Tenant?> GetTenantByDomainAsync(string domain) =>  await unitofWork.TenantRepo.GetTenantByDomainAsync( domain);
    public async Task AddTenantAsync(Tenant tenant) => await unitofWork.TenantRepo.AddTenantAsync(tenant);
}

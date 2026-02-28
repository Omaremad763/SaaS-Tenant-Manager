using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Contracts;
using Application.Contracts.IService;
using Application.DTOs;

using AutoMapper;

namespace Infra.Contracts_Implementation.Service;
public class TenantSubscriptionService(IUnitofWork unitofWork , IMapper mapper) : ITenantSubscriptionService
{
    public async Task<SubscriptionPlanDetailsDto?> GetTenantSubscriptionAsync(Guid tenantId, CancellationToken ct)
    {
        var subscription = await unitofWork.TenantSubscriptionRepo.GetTenantSubscriptionAsync(tenantId, ct);
        if (subscription == null) throw new Exception("No active subscription found for this tenant.");
        var mapping = mapper.Map<SubscriptionPlanDetailsDto>(subscription);
        return mapping;
    }
}

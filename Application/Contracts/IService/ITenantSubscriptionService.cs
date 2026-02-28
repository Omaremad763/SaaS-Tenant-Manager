using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.DTOs;

namespace Application.Contracts.IService;
public interface ITenantSubscriptionService
{
    Task<SubscriptionPlanDetailsDto?> GetTenantSubscriptionAsync(Guid tenantId, CancellationToken ct);

}

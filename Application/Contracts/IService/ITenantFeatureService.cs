using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.DTOs;

namespace Application.Contracts.IService;
public interface ITenantFeatureService
{
    Task<bool> ToggleFeatureAsync(ToggleFeatureAccessDto DTO);
    Task<FeatureStatusDto?> CheckTenantAccesedFeatures(Guid tenantId, string featureCode, CancellationToken ct);
}

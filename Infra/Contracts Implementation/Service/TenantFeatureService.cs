using Application.Contracts;
using Application.Contracts.IService;
using Application.DTOs;

using Domain.Entities;
using Domain.Entities.MasterDB;

using Infrastructure.Contracts_Implementation;

namespace Infra.Contracts_Implementation.Entites_Contracts.TenantFeatures;
public class TenantFeatureService(IUnitofWork unitOfWork) : ITenantFeatureService
{
    public async Task<bool> ToggleFeatureAsync(ToggleFeatureAccessDTO dto)
    {
        var tenantFeature = await unitOfWork.TenantFeatureRepo.GetAccessedFeatures(dto.TenantId, dto.FeatureName);

        if (tenantFeature != null)
        {
            tenantFeature.IsEnabled = dto.IsEnabled;
            tenantFeature.UpdatedAT = DateTime.UtcNow;
        }
        else
        {
            var feature = await unitOfWork.FeatureRepo.GetFeatureByNameAsync(dto.FeatureName);
            if (feature == null) return false;

            tenantFeature = new TenantFeature
            {
                TenantId = dto.TenantId,
                FeatureId = feature.Id,
                IsEnabled = dto.IsEnabled,
                CreatedAt = DateTime.UtcNow
            };
            await unitOfWork.TenantFeatureRepo.AddTenantFeatureAsync(tenantFeature);
        }
        return await unitOfWork.CommitAsync() > 0;
    }
    public async Task<FeatureStatusDto?> CheckTenantAccesedFeatures(Guid tenantId, string featureCode, CancellationToken ct)
    {
        var dto = new FeatureStatusDto(Guid.Empty, null,false,false);
        var feature = await unitOfWork.TenantFeatureRepo.GetAccessedFeatures(tenantId, featureCode);
        if (feature == null) return dto;
        dto = new FeatureStatusDto(feature.FeatureTable.Id,feature.FeatureTable.FeatureName, true,true);
        return dto;
    }
}

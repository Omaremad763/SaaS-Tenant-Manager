using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;
public class SubscriptionPlanDetailsDto
{
    public string PlanName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int MaxRequestsPerMinute { get; set; }
    public int MaxUsers { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public SubscriptionPlanDetailsDto() { }
}
//public record FeatureStatusDto(
//    string FeatureName,
//    bool IsEnabled
//);
public record GetFeatureAccessDTO(
Guid TenantId,
string FeatureName
);
public record ToggleFeatureAccessDTO(
Guid TenantId,
string FeatureName,
bool IsEnabled
);

public record TenantManagementDto(
    string TenantName,
    string PlanName,
    List<FeatureStatusDto> Features
);

public record FeatureStatusDto(
    Guid FeatureId,
    string FeatureName,
    bool IsInPlan,                
    bool IsCurrentlyEnabled         
);


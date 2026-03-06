using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;
public record ClientStatsDto
{
    public int TotalShipments { get; init; }
    public int PendingShipments { get; init; }
    public int DeliveredShipments { get; init; }
    public decimal TotalRevenue { get; init; }
    public List<TrafficStatDto> TrafficStats { get; init; } = [];
    public double SuccessRate { get; init; }
    public double AverageDeliveryTimeDays { get; init; }
}
public record TrafficStatDto
{
    public int Hour { get; init; }
    public int RequestCount { get; init; }
}
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


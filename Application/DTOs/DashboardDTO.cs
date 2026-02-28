using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;
public record SubscriptionPlanDetailsDto(
    string PlanName,
    decimal Price,
    int MonthlyRequestLimit,
    int MaxUsers,
    DateTime ExpiryDate,
    bool IsActive
);
public record FeatureStatusDto(
    string FeatureName,
    bool IsEnabled
);
public record GetFeatureAccessDTO(
Guid TenantId,
string FeatureName
);
public record ToggleFeatureAccessDTO(
Guid TenantId,
string FeatureName,
bool IsEnabled
);




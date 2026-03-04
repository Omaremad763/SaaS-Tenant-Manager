using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;
public record SystemMetricsDto(
    int TotalRequests,
    int ErrorCount,
    double AvgResponseTime,
    List<TenantUsageDto> TopTenants,      
    List<HourlyTrafficDto> TrafficStats
);

public record TenantUsageDto(
    string TenantName,
    int RequestCount
);

public record HourlyTrafficDto(
    int Hour,               
    int RequestCount,
    int ErrorCount                 
);
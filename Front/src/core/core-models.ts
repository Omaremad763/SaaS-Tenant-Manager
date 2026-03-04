export interface SubscriptionPlanDetailsDto {
  planName: string;
  price: number;
  maxRequestsPerMinute: number;
  maxUsers: number;
  endDate: string;
  isActive: boolean;
}
export interface ToggleFeatureAccessDTO {
  tenantId: string;
  featureName: string;
  isEnabled: boolean;
}
export interface FeatureStatus {
  featureId: number;
  featureName: string;
  isInPlan: boolean;
  isCurrentlyEnabled: boolean;
}

export interface TenantManagement {
  tenantName: string;
  planName: string;
  features: FeatureStatus[];
}
export interface TenantUsage {
  tenantName: string;
  requestCount: number;
}

export interface HourlyTraffic {
  hour: number;
  requestCount: number;
  errorCount: number;
}

export interface SystemMetrics {
  totalRequests: number;
  errorCount: number;
  avgResponseTime: number;
  topTenants: TenantUsage[];
  trafficStats: HourlyTraffic[];
}

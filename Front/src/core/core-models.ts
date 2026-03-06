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

export interface ShipmentItemsDTO {
  productName: string;
  quantity: number;
  price: number;
}
export interface CreateShipmentDTO {
  clientId: string;
  receiverName: string;
  receiverPhone: string;
  destination: string;
  items: ShipmentItemsDTO[];
}

export interface UpdateShipmentDTO {
  id: string;
  status: ShipmentStatus;
}

export enum ShipmentStatus {
  Pending = 'Pending',
  Shipped = 'InTransit',
  Delivered = 'Delivered',
  Cancelled = 'Cancelled',
}

export interface ShipmentDto {
  id: string;
  trackingNumber: string;
  status: string;
  receiverName: string;
  createdAt: Date;
  weight: number;
  destination: string;
}
export interface ClientDTO {
  id: string;
  name: string;
  phone: string;
  address: string;
  createdAt: string;
}

export interface UpdateClientDTO {
  id: string;
  name: string;
  phone: string;
  address: string;
}

export interface TrafficStatDto {
  hour: number;
  requestCount: number;
}

export interface ClientStatsDto {
  totalShipments: number;
  pendingShipments: number;
  deliveredShipments: number;
  totalRevenue: number;
  trafficStats: TrafficStatDto[];
  successRate: number;
  averageDeliveryTimeDays: number;
}

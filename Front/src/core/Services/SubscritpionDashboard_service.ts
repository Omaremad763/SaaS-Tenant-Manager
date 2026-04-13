import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environment';
import { ApiResponse } from '../../shared/shared_models/api-response.model';
import { ClientStatsDto, SubscriptionPlanDetailsDto, TenantManagement } from '../core-models';
@Injectable({
  providedIn: 'root',
})
export class SubscritpionDashboard_service {
  private baseUrl = `${environment.apiUrl}/SubscriptionDashboard`;
  private http = inject(HttpClient);
  getTenantSubscriptionByTenantId(
    tenantId: string,
  ): Observable<ApiResponse<SubscriptionPlanDetailsDto>> {
    return this.http.get<ApiResponse<SubscriptionPlanDetailsDto>>(
      `${this.baseUrl}/GetTenantSubscriptionByTenantId?TenantId=${tenantId}`,
    );
  }
  GetTenantSubscriptionData(): Observable<ApiResponse<TenantManagement[]>> {
    return this.http.get<ApiResponse<TenantManagement[]>>(
      `${this.baseUrl}/GetTenantSubscriptionData`,
    );
  }
  toggleFeature(payload: {
    tenantId: string;
    FeatureName: string;
    isEnabled: boolean;
  }): Observable<ApiResponse<boolean>> {
    return this.http.patch<ApiResponse<boolean>>(`${this.baseUrl}/toggleFeature`, payload);
  }
  getClientStatistics(): Observable<ApiResponse<ClientStatsDto>> {
    return this.http.get<ApiResponse<ClientStatsDto>>(`${this.baseUrl}/GetClientStatistics`);
  }
}

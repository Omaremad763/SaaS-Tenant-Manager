import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environment';
import { ApiResponse } from '../../shared/shared_models/api-response.model';

import * as dtos from '../core-models';

@Injectable({ providedIn: 'root' })
export class ShipmentsService {
  private baseUrl = `${environment.apiUrl}/Shipments`;

  constructor(private http: HttpClient) {}

  createShipment(DTO: dtos.CreateShipmentDTO): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/CreateShipment`, DTO);
  }

  updateShipmentStatus(DTO: dtos.UpdateShipmentDTO): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(`${this.baseUrl}/UpdateShipmentStatus`, DTO);
  }

  getTenantShipments(): Observable<ApiResponse<dtos.ShipmentDto[]>> {
    return this.http.get<ApiResponse<dtos.ShipmentDto[]>>(`${this.baseUrl}/GetTenantShipments`);
  }

  getClientStats(): Observable<ApiResponse<dtos.ClientStatsDto>> {
    return this.http.get<ApiResponse<dtos.ClientStatsDto>>(`${this.baseUrl}/GetClientStatistics`);
  }
}

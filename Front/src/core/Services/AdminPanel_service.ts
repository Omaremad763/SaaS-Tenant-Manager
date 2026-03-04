import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environment';
import { ApiResponse } from '../../shared/shared_models/api-response.model';
import { SystemMetrics } from '../core-models';
@Injectable({
  providedIn: 'root',
})
export class AdminPanel_service {
  private baseUrl = `${environment.apiUrl}/AdminPanel`;
  private http = inject(HttpClient);

  GetSystemMetrics(): Observable<ApiResponse<SystemMetrics>> {
    return this.http.get<ApiResponse<SystemMetrics>>(`${this.baseUrl}/GetSystemMetrics`);
  }
}

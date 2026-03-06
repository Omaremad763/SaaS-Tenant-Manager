import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environment';
import { ApiResponse } from '../../shared/shared_models/api-response.model';
import * as dtos from '../core-models';
@Injectable({
  providedIn: 'root',
})
export class ClientService {
  private baseUrl = `${environment.apiUrl}/Clients`;
  constructor(private http: HttpClient) {}

  createClient(dto: dtos.ClientDTO): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/CreateClient`, dto);
  }

  updateClient(dto: dtos.UpdateClientDTO): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(`${this.baseUrl}/UpdateClient`, dto);
  }

  deleteClient(id: string): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.baseUrl}/DeleteClient/${id}`);
  }

  getAllClients(): Observable<ApiResponse<any>> {
    return this.http.get<ApiResponse<any>>(`${this.baseUrl}/GetAllClients`);
  }
  getClientById(id: string): Observable<ApiResponse<any>> {
    return this.http.get<ApiResponse<any>>(`${this.baseUrl}/GetClientById/${id}`);
  }
}

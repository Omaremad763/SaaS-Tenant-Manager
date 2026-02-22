import { HttpClient } from '@angular/common/http';
import { inject, Injectable, NgZone, signal } from '@angular/core';
import { Router } from '@angular/router';
import { map, Observable, tap } from 'rxjs';
import { environment } from '../../environment';
import { ApiResponse } from '../../shared/shared_models/api-response.model';
import * as AuthDtos from '../shared_models/Auth-models';

export interface UserState {
  userId: string;
  username: string;
  roles: string[];
  tenantId: string; // إضافة الـ TenantId هنا مهمة جداً
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/Auth`;
  private route = inject(Router);
  private zone = inject(NgZone);

  currentUser = signal<UserState | null>(null);

  constructor() {
    this.initializeAuthState();
  }

  private initializeAuthState() {
    const token = this.getToken();
    const storedUser = localStorage.getItem('user_data');
    if (token && storedUser) {
      this.currentUser.set(JSON.parse(storedUser));
    }
  }

  // تعديل: الـ Data هنا هي الـ Token مباشرة (string)
  login(data: AuthDtos.LoginDto): Observable<string> {
    return this.http.post<ApiResponse<string>>(`${this.baseUrl}/Login`, data).pipe(
      tap((response) => {
        if (response.success && response.data) {
          this.saveToken(response.data);
          this.extractAndSaveClaims(response.data);
        }
      }),
      map((res) => res.data),
    );
  }

  registerTenant(data: AuthDtos.TenantRegistrationDto): Observable<AuthDtos.ProvisioningStatusDto> {
    return this.http
      .post<ApiResponse<AuthDtos.ProvisioningStatusDto>>(`${this.baseUrl}/RegisterTenant`, data)
      .pipe(map((res) => res.data));
  }

  registerUser(data: AuthDtos.TenantUserRegistraionDto): Observable<string> {
    return this.http
      .post<ApiResponse<string>>(`${this.baseUrl}/RegisterUser`, data)
      .pipe(map((res) => res.data));
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  saveToken(token: string): void {
    localStorage.setItem('token', token);
  }

  private extractAndSaveClaims(token: string): void {
    try {
      const payloadBase64 = token.split('.')[1];
      const payloadJson = window.atob(payloadBase64);
      const payload = JSON.parse(decodeURIComponent(escape(payloadJson)));

      // المابينج الصحيح للـ Claims اللي طالعة من الـ Backend بتاعك
      const idClaim = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';
      const emailClaim = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress';
      const roleClaim = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
      const tenantIdClaim = 'TenantId'; // الاسم اللي إنت كتبته في الـ GenerateJwt

      const rolesRaw = payload[roleClaim];

      const userState: UserState = {
        userId: payload[idClaim],
        username: payload[emailClaim],
        roles: Array.isArray(rolesRaw) ? rolesRaw : [rolesRaw],
        tenantId: payload[tenantIdClaim], // استخراج الـ TenantId من التوكن
      };

      this.currentUser.set(userState);
      localStorage.setItem('user_data', JSON.stringify(userState));
    } catch (error) {
      console.error('Error decoding token', error);
      this.logout();
    }
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user_data');
    this.currentUser.set(null);
    this.zone.run(() => this.route.navigate(['/login']));
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }
}

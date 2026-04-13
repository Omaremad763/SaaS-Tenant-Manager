export interface TenantRegistrationDto {
  name: string;
  slug: string;
  planId: number;
  password: string;
  email: string;
  tenantDomain?: string | null;
}
export interface TenantUserRegistraionDto {
  email: string;
  password: string;
}
export interface LoginDto {
  email: string;
  password: string;
}

export interface ProvisioningStatusDto {
  tenantId: string;
  status: 'Provisioning' | 'Active' | 'Failed';
  message?: string;
}

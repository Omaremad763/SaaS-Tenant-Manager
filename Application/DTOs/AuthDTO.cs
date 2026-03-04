namespace Application.DTOs;
public record TenantRegistrationDto(string Name, string Slug, int PlanId,string Password,string Email,string? TenantDomain);
public record ProvisioningStatusDto(Guid? TenantId, string Status, string Message);
public record GetTenantDto( Guid Id, string Name,string Slug,bool IsActive);

public record TenantUserRegistraionDto(string Email, string Password );
public record LoginDto(string Email, string Password );



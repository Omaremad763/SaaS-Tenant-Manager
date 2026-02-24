using Application.DTOs;

namespace Application.Contracts.Auth
{
    public interface IUserService
    {
        Task<string> Login(LoginDto dto);
        Task<ProvisioningStatusDto> RegisterTenantAdmin(TenantRegistrationDto dto);
        Task<string> RegisterTenantUser(TenantUserRegistraionDto dto);
        Task<SystemAdminSeedResultEnum> EnsureSystemAdminAsync();
    }
}

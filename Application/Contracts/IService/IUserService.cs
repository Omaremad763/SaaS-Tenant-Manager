using Application.DTOs;
using Application.Enums;

namespace Application.Contracts.IService
{
    public interface IUserService
    {
        Task<string> Login(LoginDto dto);
        Task<ProvisioningStatusDto> RegisterTenantAdmin(TenantRegistrationDto dto);
        Task<string> RegisterTenantUser(TenantUserRegistraionDto dto);
        Task<EnumSystemAdminSeedResult> EnsureSystemAdminAsync();
    }
}

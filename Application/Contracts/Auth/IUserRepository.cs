using Domain.Entities;

using Microsoft.AspNetCore.Identity;

namespace Application.Contracts.Auth
{
    public interface IUserRepository
    {
        Task<IdentityResult> CreateUserWithRoleAsync(User user, string password, string roleName);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IList<string>> GetRolesAsync(User user);
        Task<User?> FindByEmailAsync( string Email);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task AddTenantAsync(Tenant tenant);
        Task<Tenant?> GetTenantBySlugAndDomainAsync(string slug, string domain);
        Task<Tenant?> GetTenantByDomainAsync(string domain);
        Task<SystemAdminSeedResultEnum> EnsureSystemAdminAsync();
     }
}

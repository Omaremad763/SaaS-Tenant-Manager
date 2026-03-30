using Application.Enums;

using Domain.Entities.MasterDB;

using Microsoft.AspNetCore.Identity;

namespace Application.Contracts.IRepo
{
    public interface IUserRepository
    {
        Task<IdentityResult> CreateUserWithRoleAsync(User user, string password, string roleName);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IList<string>> GetRolesAsync(User user);
        Task<User?> FindByEmailAsync( string Email);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<EnumSystemAdminSeedResult> EnsureSystemAdminAsync();
     }
}

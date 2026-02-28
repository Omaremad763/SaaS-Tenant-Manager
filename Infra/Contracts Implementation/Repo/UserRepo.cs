using Application.Contracts.IRepo;
using Application.Enums;

using Domain.Entities.MasterDB;

using Infra.Persistence.Contexts;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infra.Contracts_Implementation.Auth_Page;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<UserRoles> _roleManager;
    private readonly MasterDbContext _context;
    public UserRepository(
        UserManager<User> userManager,
        RoleManager<UserRoles> roleManager,
        MasterDbContext context)
    {
        _userManager = userManager;
        _context = context;
        _roleManager = roleManager;
    }

    public async Task<IdentityResult> CreateUserWithRoleAsync(User user, string password, string roleName)
    {
        var admindomin = Environment.GetEnvironmentVariable("SaasAdminDomain");
        if (roleName == "SystemAdmin") { user.TenantDomain = admindomin; }
        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            await _roleManager.CreateAsync(new UserRoles
            {
                Name = roleName,
                NormalizedName = roleName.ToUpper()
            });
        }
        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }
        return result;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }
    public async Task<User?> FindByEmailAsync(string Email)
    {
        return await _userManager.FindByEmailAsync(Email);
    }
    public async Task<IList<string>> GetRolesAsync(User user)
    {
        return await _userManager.GetRolesAsync(user);
    }
    public async Task<SystemAdminSeedResultEnum> EnsureSystemAdminAsync()
    {
        var existingAdmin = await _userManager.GetUsersInRoleAsync("SystemAdmin");

        if (existingAdmin.Any()) return SystemAdminSeedResultEnum.AlreadyExists;
        var password = Environment.GetEnvironmentVariable("SystemAdminPassword");
        var email = Environment.GetEnvironmentVariable("SystemAdminEmail");

        var user = new User
        {
            UserName = email,
            Email = email,
        };
        await CreateUserWithRoleAsync(user, password, "SystemAdmin");
        int saving = _context.SaveChanges();
        if (saving<0) return SystemAdminSeedResultEnum.Failed;
        return SystemAdminSeedResultEnum.Created;
    }

}



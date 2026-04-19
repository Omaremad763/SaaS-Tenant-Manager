using Application.Contracts.IRepo;
using Application.Enums;

using Domain.Entities.MasterDB;

using Infra.Persistence.Contexts;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Infra.Contracts_Implementation.Auth_Page;

public class UserRepository(
    UserManager<User> userManager,
    RoleManager<UserRoles> roleManager,
    MasterDbContext context,
    IConfiguration config
    ) 
    : IUserRepository
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<UserRoles> _roleManager = roleManager;
    private readonly MasterDbContext _context = context;

    public async Task<IdentityResult> CreateUserWithRoleAsync(User user, string password, string roleName)
    {
        var AdminDomain = config["Adminsettings:SaasAdminDomain"];
        if (roleName == "SystemAdmin") { user.TenantDomain = AdminDomain; }
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

    public async Task<EnumSystemAdminSeedResult> EnsureSystemAdminAsync()
    {
        var existingAdmin = await _userManager.GetUsersInRoleAsync("SystemAdmin");

        if (existingAdmin.Any()) return EnumSystemAdminSeedResult.AlreadyExists;
        var password = config["Adminsettings:SystemAdminPassword"];
        var email = config["Adminsettings:SystemAdminEmail"];

        var user = new User
        {
            UserName = email,
            Email = email,
        };
        await CreateUserWithRoleAsync(user, password, "SystemAdmin");
        int saving = _context.SaveChanges();
        if (saving < 0) return EnumSystemAdminSeedResult.Failed;
        return EnumSystemAdminSeedResult.Created;
    }
}
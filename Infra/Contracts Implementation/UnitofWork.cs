using Application.Contracts;
using Application.Contracts.Auth;

using Domain.Entities;

using Infra.Contracts_Implementation.Auth_Page;
using Infra.Persistence;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Contracts_Implementation
{
    public class UnitofWork(ApplicationDbContext context, IConfiguration configuration, UserManager<User> _userManager,RoleManager<UserRoles> roleManager) : IUnitofWork
    {
        public IUserRepository userRepository =>  new UserRepository( _userManager, roleManager, context);


        public async Task<int> CommitAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();GC.SuppressFinalize(this);
        }
    }
}

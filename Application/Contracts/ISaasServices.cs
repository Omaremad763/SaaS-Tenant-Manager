using Application.Contracts.Auth;

namespace Application.Contracts
{
    public interface ISaasServices
    {
        public  IDbMigrationService DbMigrationService { get; }
        public IUserService UserService { get; }

    }
}
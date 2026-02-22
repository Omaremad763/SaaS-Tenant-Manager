using Application.Contracts.Auth;

namespace Application.Contracts;
public interface IUnitofWork:IDisposable
{
    IUserRepository userRepository { get; }
    Task<int> CommitAsync();
}


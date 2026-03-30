using Application.Contracts.IRepo;

using Domain.Entities.TenantDBEntities;

using Infra.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;

namespace Infra.Contracts_Implementation.Repo;

public class ClientRepository(TenantDbContext context) : IClientRepository
{
    private readonly TenantDbContext _context = context;

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _context.Clients.AsNoTracking().ToListAsync();
    }

    public async Task<Client?> GetByIdAsync(Guid id)
    {
        return await _context.Clients.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Client client)
    {
        await _context.Clients.AddAsync(client);
    }

    public void Update(Client client)
    {
        _context.Clients.Update(client);
    }

    public void Delete(Client client)
    {
        _context.Clients.Remove(client);
    }
}
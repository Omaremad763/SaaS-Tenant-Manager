using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Entities.TenantDBEntities;

namespace Application.Contracts.IRepo;
public interface IClientRepository
{
    Task<IEnumerable<Client>> GetAllAsync();
    Task<Client?> GetByIdAsync(Guid id);
    Task AddAsync(Client client);
    void Update(Client client);
    void Delete(Client client);
}
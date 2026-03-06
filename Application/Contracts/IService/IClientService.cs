using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.DTOs;

using Domain.Entities.TenantDBEntities;

namespace Application.Contracts.IService;
public interface IClientService
{
    Task<IEnumerable<ClientDTO>> GetAllClientsAsync();
    Task<ClientDTO?> GetClientByIdAsync(Guid id);
    Task CreateClientAsync(ClientDTO DTO);
   Task UpdateClientAsync(UpdateClientDTO DTO);
    Task DeleteClientAsync(ClientDTO DTO);
}

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
    Task<IEnumerable<ClientDto>> GetAllClientsAsync();
    Task<ClientDto?> GetClientByIdAsync(Guid id);
    Task CreateClientAsync(ClientDto DTO);
   Task UpdateClientAsync(UpdateClientDto DTO);
    Task DeleteClientAsync(ClientDto DTO);
}

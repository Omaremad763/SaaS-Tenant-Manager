using Application.Contracts;
using Application.Contracts.IService;
using Application.DTOs;

using AutoMapper;

using Domain.Entities.TenantDBEntities;

namespace Infra.Contracts_Implementation.Repo;

public class ClientService(IUnitofWork unitofWork, IMapper mapper) : IClientService
{
    private readonly IUnitofWork unitofWork = unitofWork;
    private readonly IMapper mapper = mapper;

    public async Task<IEnumerable<ClientDto>> GetAllClientsAsync()
    {
        var data = await unitofWork.ClientRepository.GetAllAsync();
        var mapping = mapper.Map<IEnumerable<ClientDto>>(data);
        return mapping;
    }

    public async Task<ClientDto?> GetClientByIdAsync(Guid id)
    {
        var Data = await unitofWork.ClientRepository.GetByIdAsync(id);
        var mapping = mapper.Map<ClientDto>(Data);
        return mapping;
    }

    public async Task CreateClientAsync(ClientDto DTO)
    {
        var mapping = mapper.Map<Client>(DTO);
        await unitofWork.ClientRepository.AddAsync(mapping);
        await unitofWork.TenantCommitAsync();
    }

    public async Task UpdateClientAsync(UpdateClientDto DTO)
    {
        var mapping = mapper.Map<Client>(DTO);
        unitofWork.ClientRepository.Update(mapping);
        await unitofWork.TenantCommitAsync();
    }

    public async Task DeleteClientAsync(ClientDto DTO)
    {
        var mapping = mapper.Map<Client>(DTO);
        var client = await unitofWork.ClientRepository.GetByIdAsync(DTO.Id);
        if (client != null)
        {
            unitofWork.ClientRepository.Delete(mapping);
            await unitofWork.TenantCommitAsync();
        }
    }
}
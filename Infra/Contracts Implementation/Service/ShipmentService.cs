using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Contracts;
using Application.Contracts.IService;
using Application.DTOs;

namespace Infra.Contracts_Implementation.Service;
public class ShipmentService(IUnitofWork unitofWork) : IShipmentService
{
    public async Task<bool> CreateShipment(CreateShipmentDTO DTO, CancellationToken ct)
    {
        return await unitofWork.shipmentRepo.CreateShipment(DTO, ct);
    }

    public async Task<ClientStatsDto> GetClientStatistics(CancellationToken ct)
    {
        return await unitofWork.shipmentRepo.GetClientStatistics(ct);
    }

    public async Task<List<ShipmentDto?>> GetTenantShipments(CancellationToken ct)
    {
        return await unitofWork.shipmentRepo.GetTenantShipments(ct);
    }

    public async Task<bool> UpdateShipment(UpdateShipmentDTO DTO, CancellationToken ct)
    {
        return await unitofWork.shipmentRepo.UpdateShipment(DTO, ct);
    }
}

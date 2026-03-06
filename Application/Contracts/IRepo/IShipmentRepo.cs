using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.DTOs;

namespace Application.Contracts.IRepo;
public interface IShipmentRepo
{
Task<bool> CreateShipment(CreateShipmentDTO DTO, CancellationToken ct);

Task<bool> UpdateShipment(UpdateShipmentDTO DTO, CancellationToken ct);

Task<List<ShipmentDto>> GetTenantShipments(CancellationToken ct);

Task<ClientStatsDto> GetClientStatistics(CancellationToken ct);
    
}

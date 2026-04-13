using Application.Contracts.IRepo;
using Application.DTOs;

using Domain.Entities.TenantDBEntities;

using IdGen;

using Infra.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;

namespace Infra.TenantDBRepo;

public class ShipmentRepo(TenantDbContext _Tenantcontext, IIdGenerator<long> _idGenerator) : IShipmentRepo
{
    private async Task<List<TrafficStatDto>> GetHourlyTrafficAsync(CancellationToken ct)
    {
        var yesterday = DateTime.UtcNow.AddDays(-1);

        var stats = await _Tenantcontext.Shipments
            .Where(s => s.CreatedAt >= yesterday)
            .GroupBy(s => s.CreatedAt.Hour)
            .Select(g => new TrafficStatDto
            {
                Hour = g.Key,
                RequestCount = g.Count()
            })
            .ToListAsync(ct);

        var fullDayStats = Enumerable.Range(0, 24).Select(hour =>
        {
            var existingStat = stats.FirstOrDefault(s => s.Hour == hour);
            return existingStat ?? new TrafficStatDto { Hour = hour, RequestCount = 0 };
        })
        .OrderBy(s => s.Hour)
        .ToList();

        return fullDayStats;
    }

    public async Task<bool> CreateShipment(CreateShipmentDto DTO, CancellationToken ct)
    {
        decimal PlanRate = 50m;
        long snowflakeId = _idGenerator.CreateId();
        var domainItems = DTO.Items.Select(i => new ShipmentItem
        {
            ProductName = i.ProductName,
            Quantity = i.Quantity,
            UnitPrice = i.Price
        }).ToList();
        var shipment = Shipment.Create(
            snowflakeId,
            DTO.ReceiverName,
            DTO.ReceiverPhone,
            DTO.Destination,
            DTO.ClientId,
        domainItems,
        PlanRate
);
        await _Tenantcontext.Shipments.AddAsync(shipment, ct);
        await _Tenantcontext.ShipmentItems.AddRangeAsync(domainItems, ct);
        var saving = await _Tenantcontext.SaveChangesAsync(ct);
        return saving >= 0;
    }

    public async Task<bool> UpdateShipment(UpdateShipmentDto DTO, CancellationToken ct)
    {
        var shipment = await _Tenantcontext.Shipments.FirstOrDefaultAsync(s => s.Id == DTO.Id, ct);
        if (shipment == null) return false;
        shipment.Status = DTO.Status;
        shipment.CreatedAt = DateTime.UtcNow;
        return await _Tenantcontext.SaveChangesAsync(ct) > 0;
    }

    public async Task<List<ShipmentDto>> GetTenantShipments(CancellationToken ct)
    {
        return await _Tenantcontext.Shipments
                .AsNoTracking()
                .Select(s => new ShipmentDto
                {
                    Id = s.Id,
                    TrackingNumber = s.Trackingnumber,
                    Status = s.Status,
                    ReceiverName = s.ReceiverName,
                    Destination = s.DeliveryAddress,
                    CreatedAt = s.CreatedAt
                })
                .ToListAsync(ct);
    }

    public async Task<ClientStatsDto> GetClientStatistics(CancellationToken ct)
    {
        var total = await _Tenantcontext.Shipments.CountAsync(ct);
        var delivered = await _Tenantcontext.Shipments.CountAsync(s => s.Status == ShipmentStatus.Delivered, ct);
        return new ClientStatsDto
        {
            TotalShipments = total,
            DeliveredShipments = delivered,
            SuccessRate = total > 0 ? (double)delivered / total * 100 : 0,
            TrafficStats = await GetHourlyTrafficAsync(ct)
        };
    }
}
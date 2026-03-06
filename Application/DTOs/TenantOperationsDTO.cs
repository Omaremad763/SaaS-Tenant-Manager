using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Entities.TenantDBEntities;

namespace Application.DTOs;
public record ShipmentDto
{
    public Guid Id { get; init; }
    public long TrackingNumber { get; init; }
    public string ReceiverName { get; init; } = string.Empty;
    public string Destination { get; init; } = string.Empty;
    public ShipmentStatus Status { get; init; } = ShipmentStatus.Pending;
    public DateTime CreatedAt { get; init; }
    public DateTime? LastUpdate { get; init; }
}
public record ShipmentItemsDTO(
    string ProductName,
    int Quantity,
    decimal Price
);
public record CreateShipmentDTO
(
    string ReceiverName,
    string ReceiverPhone,
    string Destination,
    List<ShipmentItemsDTO> Items,
    Guid ClientId
);
public record UpdateShipmentDTO
(
    Guid Id,
    ShipmentStatus Status
);

public class ClientDTO
{
    public Guid Id { get; set; }=new Guid();
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class UpdateClientDTO
{
    public Guid Id { get; set; } 
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
}
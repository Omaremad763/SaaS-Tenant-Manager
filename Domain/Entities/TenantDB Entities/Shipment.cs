using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.TenantDB_Entities;
public class Shipment
{
    public Guid Id { get; set; }
    public string TrackingNumber { get; set; }     
    public string ReceiverName { get; set; }
    public string ReceiverPhone { get; set; }
    public string DeliveryAddress { get; set; }
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;
    public decimal TotalWeight { get; set; }
    public decimal ShippingFees { get; set; }   
    public Guid ClientId { get; set; }
    public Client Client { get; set; }
    public ICollection<ShipmentItem> Items { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
public enum ShipmentStatus
{
    Pending,
    InTransit,
    Delivered,
    Cancelled
}


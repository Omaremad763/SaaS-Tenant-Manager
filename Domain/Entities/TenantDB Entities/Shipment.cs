using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.TenantDBEntities;
public class Shipment
{
    public Guid Id { get; set; } = new Guid();
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid ClientId { get; private set; }
    public Client Client { get; }
    public ICollection<ShipmentItem> Items { get; private set; }
    public long Trackingnumber { get; private set; }
    public string? ReceiverName { get; private set; }
    public string? ReceiverPhone { get; private set; }
    public string? DeliveryAddress { get; private set; }
    public decimal TotalWeight { get; private set; }
    public decimal ShippingFees { get; private set; }
    private Shipment() { }
    public static Shipment Create
        (long snowflakeId, string receiverName,
         string ReceiverPhone, string DeliveryAddress,
        Guid ClientId,List<ShipmentItem> items,decimal planRate)
    {
        var shipment = new Shipment
        {
            Id = Guid.NewGuid(),
            Trackingnumber = snowflakeId,
            ReceiverPhone= ReceiverPhone,
            DeliveryAddress= DeliveryAddress,
            ClientId= ClientId,
            ReceiverName = receiverName,
            Items = items

        };

        shipment.CalculateTotals(planRate);

        return shipment;
    }

    private void CalculateTotals(decimal planRate)
    {
        this.TotalWeight = this.Items.Sum(x => x.Quantity);
        this.ShippingFees = this.TotalWeight * planRate;
    }

}
public enum ShipmentStatus
{
    Pending,
    InTransit,
    Delivered,
    Cancelled
}


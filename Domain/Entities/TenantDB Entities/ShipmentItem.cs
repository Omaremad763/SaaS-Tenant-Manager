using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.TenantDBEntities;
public class ShipmentItem
{
    public Guid Id { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }        
    public Guid ShipmentId { get; set; }
    public Shipment Shipment { get; set; }
}

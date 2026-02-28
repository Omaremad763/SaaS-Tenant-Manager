using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.TenantDB_Entities;
public class Client
{
    public Guid Id { get; set; }
    public string Name { get; set; }     
    public string Phone { get; set; }
    public string Address { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Shipment> Shipments { get; set; }
}

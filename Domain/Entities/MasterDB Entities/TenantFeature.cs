using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MasterDB;
public class TenantFeature
{
    public Guid TenantId { get; set; }
    public Guid FeatureId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAT { get; set; }
    public bool IsEnabled { get; set; } = true;
    public Tenant TenantTable { get; }
    public Feature FeatureTable { get; set; }
}

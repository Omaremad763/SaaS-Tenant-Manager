using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MasterDB;
public class Tenant : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;       
    public string ConnectionString { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string TenantDomain { get; set; } = string.Empty;
    public ICollection<TenantFeature> TenantFeatures { get; set; } = [];
    public ICollection<ApiLog> ApiLogs { get; set; } = [];
    public TenantSubscription? TenantSubscriptionTable { get; set; }

}


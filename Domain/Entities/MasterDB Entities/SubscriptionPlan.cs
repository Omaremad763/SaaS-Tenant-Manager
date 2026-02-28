using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MasterDB;
public class SubscriptionPlan : BaseEntity
{
    public string PlanName { get; set; } = string.Empty;
    public int PlanTier { get; set; } 
    public decimal Price { get; set; }
    public int MaxRequestsPerMinute { get; set; } 
    public int MaxUsers { get; set; }      
    public long StorageLimitGb { get; set; }
    public ICollection<PlanFeature> PlanFeatures { get; set; } = [];
    public ICollection<TenantSubscription> TenantSubscriptionTable { get; set; } = [];

}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MasterDB;
public class TenantSubscription
{
    public Guid TenantId { get; set; }
    public Guid SubscriptionPlanId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public Tenant TenantTable { get; set; } = null!;
    public SubscriptionPlan SubscriptionPlanTable { get; set; } = null!;
}


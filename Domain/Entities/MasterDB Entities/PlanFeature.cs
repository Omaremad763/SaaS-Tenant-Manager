using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Domain.Entities.MasterDB;
public class PlanFeature
{
    public Guid SubscriptionPlanId { get; set; }
    public Guid FeatureId { get; set; }
    public bool IsEnabledForPlan { get; set; } = true;
    public  SubscriptionPlan SubscriptionPlanTable { get; set; } = null!;
    public  Feature FeatureTable { get; set; } = null!;
}
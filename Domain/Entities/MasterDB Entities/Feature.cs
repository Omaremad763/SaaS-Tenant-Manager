using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MasterDB;
public class Feature : BaseEntity
{
    public string FeatureName { get; set; } = string.Empty; 
    public bool IsEnabledForSpecificTenant { get; set; }
    public string FeatureCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<TenantFeature> TenantFeatureTable { get; set; }= [];
    public ICollection<PlanFeature> PlanFeatureTable { get; set; } = [];

}


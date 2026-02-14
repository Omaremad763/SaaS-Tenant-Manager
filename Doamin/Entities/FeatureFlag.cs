using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

    public class FeatureFlag : BaseEntity
    {
        public string FeatureName { get; set; } = string.Empty; // e.g., "AiInsights"
        public bool IsEnabledGlobal { get; set; } = true;
    }


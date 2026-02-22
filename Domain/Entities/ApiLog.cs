using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;
    public class ApiLog : BaseEntity
    {
        public Guid TenantId { get; set; }
        public string Endpoint { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public long DurationMs { get; set; }
        public string IpAddress { get; set; } = string.Empty;

        // Navigation Property
        public virtual Tenant Tenant { get; set; } = null!;
    }
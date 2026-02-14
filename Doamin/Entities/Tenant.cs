using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;
public class Tenant : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty; // Unique identifier in URL (e.g., "google")
    public string ConnectionString { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    // Relationships
    public virtual ICollection<ApiLog> ApiLogs { get; set; } = new List<ApiLog>();
}


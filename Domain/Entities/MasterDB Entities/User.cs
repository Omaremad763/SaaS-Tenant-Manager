using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.MasterDB
{
    public class User : IdentityUser<Guid>
    {
        public string TenantDomain { get; set; }

        public Guid? TenantId { get; set; }

    }
}

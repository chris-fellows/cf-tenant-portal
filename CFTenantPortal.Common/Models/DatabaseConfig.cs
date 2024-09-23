using CFTenantPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Models
{
    public class DatabaseConfig : IDatabaseConfig
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Models
{
    public class PropertyFilter
    {
        public string? Search { get; set; }

        public List<string> PropertyGroupIds { get; set; }
        
        public List<string> PropertyOwnerIds { get; set; }

        public int PageItems { get; set; }

        public int PageNo { get; set; }
    }
}

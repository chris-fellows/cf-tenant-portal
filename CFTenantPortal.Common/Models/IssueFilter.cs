using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Models
{
    public class IssueFilter
    {
        public List<string> References { get; set; }
        
        public List<string> IssueStatusIds { get; set; }

        public List<string> IssueTypeIds { get; set; }

        public int PageItems { get; set; }

        public int PageNo { get; set; }
    }
}

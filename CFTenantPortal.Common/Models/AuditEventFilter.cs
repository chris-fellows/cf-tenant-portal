using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Audit event filter
    /// </summary>
    public class AuditEventFilter
    {
        public DateTimeOffset StartCreatedDateTime { get; set; }

        public DateTimeOffset EndCreatedDateTime { get; set; }

        public List<string> AuditEventTypeIds { get; set; }

        public List<string> PropertyGroupIds { get; set; }

        public List<string> PropertyIds { get; set; }

        public List<string> PropertyOwnerIds { get; set; }        

        public int PageItems { get; set; }

        public int PageNo { get; set; }
    }
}

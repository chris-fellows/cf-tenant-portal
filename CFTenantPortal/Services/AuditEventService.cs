using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;

namespace CFTenantPortal.Services
{
    public class AuditEventService : IAuditEventService
    {
        public Task Add(AuditEvent auditEvent)
        {
            return Task.CompletedTask;
        }
    }
}

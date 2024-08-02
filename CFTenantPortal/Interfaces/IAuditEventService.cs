using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IAuditEventService
    {
        Task Add(AuditEvent auditEvent);
    }
}

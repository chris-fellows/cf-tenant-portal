using CFTenantPortal.Enums;
using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IAuditEventTypeService : IEntityWithIDService<AuditEventType, string>
    {
        //Task<List<AuditEventType>> GetAll();

        //Task<AuditEventType> GetById(string id);

        Task<AuditEventType> GetByEnum(AuditEventTypes auditEventType);
    }
}

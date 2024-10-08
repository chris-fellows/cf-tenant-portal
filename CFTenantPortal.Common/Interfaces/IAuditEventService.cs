﻿using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IAuditEventService : IEntityWithIDService<AuditEvent, string>
    {
        //Task Add(AuditEvent auditEvent);

        Task<List<AuditEvent>> GetByFilterAsync(AuditEventFilter auditEventFilter);
    }
}

using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;

namespace CFTenantPortal.Seed1
{
    public class AuditEventSeed1 : IEntityList<AuditEvent>
    {
        public Task<List<AuditEvent>> ReadAllAsync()
        {
            var entities = new List<AuditEvent>();

            return Task.FromResult(entities);
        }

        public Task WriteAllAsync(List<AuditEvent> entities)
        {
            return Task.CompletedTask;
        }
    }
}

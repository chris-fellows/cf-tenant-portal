using CFTenantPortal.Enums;
using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;

namespace CFTenantPortal.Seed1
{
    public class AuditEventTypeSeed1 : IEntityList<AuditEventType>
    {
        public Task<List<AuditEventType>> ReadAllAsync()
        {
            var entities = new List<AuditEventType>();

            entities.Add(new AuditEventType()
            {                
                Description = "Document added",
                EventType = AuditEventTypes.DocumentAdded
            });

            entities.Add(new AuditEventType()
            {             
                Description = "Employee added",
                EventType = AuditEventTypes.EmployeeAdded
            });

            entities.Add(new AuditEventType()
            {             
                Description = "Issue added",
                EventType = AuditEventTypes.IssueAdded
            });

            entities.Add(new AuditEventType()
            {             
                Description = "Property added",
                EventType = AuditEventTypes.PropertyAdded
            });

            entities.Add(new AuditEventType()
            {             
                Description = "Property group added",
                EventType = AuditEventTypes.PropertyGroupAdded
            });

            entities.Add(new AuditEventType()
            {             
                Description = "Property owner added",
                EventType = AuditEventTypes.PropertyOwnerAdded
            });

            return Task.FromResult(entities);
        }

        public Task WriteAllAsync(List<AuditEventType> entities)
        {
            return Task.CompletedTask;
        }
    }
}

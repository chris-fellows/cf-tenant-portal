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
                Description = "Document deleted",
                EventType = AuditEventTypes.DocumentDeleted
            });

            entities.Add(new AuditEventType()
            {
                Description = "Document updated",
                EventType = AuditEventTypes.DocumentUpdated
            });

            entities.Add(new AuditEventType()
            {             
                Description = "Employee added",
                EventType = AuditEventTypes.EmployeeAdded
            });

            entities.Add(new AuditEventType()
            {
                Description = "Employee deleted",
                EventType = AuditEventTypes.EmployeeDeleted
            });

            entities.Add(new AuditEventType()
            {
                Description = "Employee updated",
                EventType = AuditEventTypes.EmployeeUpdated
            });            

            entities.Add(new AuditEventType()
            {             
                Description = "Issue added",
                EventType = AuditEventTypes.IssueAdded
            });

            entities.Add(new AuditEventType()
            {
                Description = "Issue deleted",
                EventType = AuditEventTypes.IssueDeleted
            });

            entities.Add(new AuditEventType()
            {
                Description = "Issue updated",
                EventType = AuditEventTypes.IssueUpdated
            });

            entities.Add(new AuditEventType()
            {
                Description = "Message added",
                EventType = AuditEventTypes.MessageAdded
            });

            entities.Add(new AuditEventType()
            {
                Description = "Message deleted",
                EventType = AuditEventTypes.MessageDeleted
            });

            entities.Add(new AuditEventType()
            {
                Description = "Message updated",
                EventType = AuditEventTypes.MessageUpdated
            });

            entities.Add(new AuditEventType()
            {             
                Description = "Property added",
                EventType = AuditEventTypes.PropertyAdded
            });

            entities.Add(new AuditEventType()
            {
                Description = "Property deleted",
                EventType = AuditEventTypes.PropertyDeleted
            });

            entities.Add(new AuditEventType()
            {
                Description = "Property updated",
                EventType = AuditEventTypes.PropertyUpdated
            });

            entities.Add(new AuditEventType()
            {             
                Description = "Property group added",
                EventType = AuditEventTypes.PropertyGroupAdded
            });

            entities.Add(new AuditEventType()
            {
                Description = "Property group deleted",
                EventType = AuditEventTypes.PropertyGroupDeleted
            });

            entities.Add(new AuditEventType()
            {
                Description = "Property group updated",
                EventType = AuditEventTypes.PropertyGroupUpdated
            });

            entities.Add(new AuditEventType()
            {             
                Description = "Property owner added",
                EventType = AuditEventTypes.PropertyOwnerAdded
            });

            entities.Add(new AuditEventType()
            {
                Description = "Property owner deleted",
                EventType = AuditEventTypes.PropertyOwnerDeleted
            });

            entities.Add(new AuditEventType()
            {
                Description = "Property owner updated",
                EventType = AuditEventTypes.PropertyOwnerUpdated
            });            

            entities.Add(new AuditEventType()
            {
                Description = "User logged in",
                EventType = AuditEventTypes.UserLoggedIn
            });

            entities.Add(new AuditEventType()
            {
                Description = "User logged out",
                EventType = AuditEventTypes.UserLoggedOut
            });

            return Task.FromResult(entities);
        }

        public Task WriteAllAsync(List<AuditEventType> entities)
        {
            return Task.CompletedTask;
        }
    }
}

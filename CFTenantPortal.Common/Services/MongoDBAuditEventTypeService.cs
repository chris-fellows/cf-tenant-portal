using CFTenantPortal.Enums;
using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using MongoDB.Driver;

namespace CFTenantPortal.Services
{
    public class MongoDBAuditEventTypeService : MongoDBBaseService<AuditEventType>, IAuditEventTypeService
    {
        public MongoDBAuditEventTypeService(IDatabaseConfig databaseConfig) : base(databaseConfig, "audit_event_types")
        {
    
        }

        public Task<AuditEventType?> GetByIdAsync(string id)
        {
            return _entities.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        //public Task<AccountTransaction?> GetByNameAsync(string name)
        //{
        //    return _entities.Find(x => x.Name == name).FirstOrDefaultAsync();
        //}

        public Task DeleteByIdAsync(string id)
        {
            return _entities.DeleteOneAsync(id);
        }

        //public Task<List<AuditEventType>> GetAll()
        //{
        //    return Task.FromResult(GetAllInternal());
        //}

        //public Task<AuditEventType> GetById(string id)
        //{
        //    return Task.FromResult(GetAllInternal().FirstOrDefault(e => e.Id == id));
        //}

        public Task<AuditEventType> GetByEnum(AuditEventTypes auditEventType)
        {
            return Task.FromResult(GetAll().FirstOrDefault(e => e.EventType == auditEventType));
        }

        //private List<AuditEventType> GetAllInternal()
        //{
        //    var auditEventTypes = new List<AuditEventType>();

        //    auditEventTypes.Add(new AuditEventType()
        //    {
        //        Id = "1",
        //        Description = "Document added",
        //        EventType = AuditEventTypes.DocumentAdded
        //    });

        //    auditEventTypes.Add(new AuditEventType()
        //    {
        //        Id = "1",
        //        Description = "Employee added",
        //        EventType = AuditEventTypes.EmployeeAdded
        //    });

        //    auditEventTypes.Add(new AuditEventType()
        //    {
        //        Id = "1",
        //        Description = "Issue added",
        //        EventType = AuditEventTypes.IssueAdded
        //    });

        //    auditEventTypes.Add(new AuditEventType()
        //    {
        //        Id = "1",
        //        Description = "Property added",
        //        EventType = AuditEventTypes.PropertyAdded
        //    });

        //    auditEventTypes.Add(new AuditEventType()
        //    {
        //        Id = "1",
        //        Description = "Property group added",
        //        EventType = AuditEventTypes.PropertyGroupAdded
        //    });

        //    auditEventTypes.Add(new AuditEventType()
        //    {
        //        Id = "1",
        //        Description = "Property owner added",
        //        EventType = AuditEventTypes.PropertyOwnerAdded
        //    });

        //    return auditEventTypes;
        //}
    }
}

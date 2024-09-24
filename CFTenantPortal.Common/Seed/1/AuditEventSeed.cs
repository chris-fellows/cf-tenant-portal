using CFTenantPortal.Enums;
using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using static System.Formats.Asn1.AsnWriter;

namespace CFTenantPortal.Seed1
{
    public class AuditEventSeed1 : IEntityList<AuditEvent>
    {
        public Task<List<AuditEvent>> ReadAllAsync()
        {
            var entities = new List<AuditEvent>();
        //    // Add test audit events
        //    var auditEventTypeService = scope.ServiceProvider.GetRequiredService<IAuditEventTypeService>();
        //    var auditEventService = scope.ServiceProvider.GetRequiredService<IAuditEventService>();
        //    var documentService = scope.ServiceProvider.GetRequiredService<IDocumentService>();
        //    var issueService = scope.ServiceProvider.GetRequiredService<IIssueService>();
        //    var propertyService = scope.ServiceProvider.GetRequiredService<IPropertyService>();
        //    var propertyOwnerService = scope.ServiceProvider.GetRequiredService<IPropertyOwnerService>();
        //    var propertyGroupService = scope.ServiceProvider.GetRequiredService<IPropertyGroupService>();
        //    var systemValueTypeService = scope.ServiceProvider.GetRequiredService<ISystemValueTypeService>();

        //    var auditEventTypes = auditEventTypeService.GetAll().ToList();
        //    var documents = documentService.GetAll().ToList();
        //    var issues = issueService.GetAll().ToList();
        //    var properties = propertyService.GetAll().ToList();
        //    var propertyOwners = propertyOwnerService.GetAll().ToList();
        //    var propertyGroups = propertyGroupService.GetAll().ToList();
        //    var systemValueTypes = systemValueTypeService.GetAll().ToList();

        //    var auditEvent1 = new AuditEvent()
        //    {
        //        CreatedDateTime = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromMinutes(30)),
        //        EventTypeId = auditEventTypes[0].Id,
        //        Parameters = new List<AuditEventParameter>()
        //{
        //    new AuditEventParameter()
        //    {
        //        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.PropertyId).Id,
        //        Value = properties[0].Id
        //    },
        //     new AuditEventParameter()
        //    {
        //        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.DocumentId).Id,
        //        Value = documents[0].Id
        //    }
        //}
        //    };
        //    await auditEventService.AddAsync(auditEvent1);

        //    var auditEvent2 = new AuditEvent()
        //    {
        //        CreatedDateTime = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromMinutes(60)),
        //        EventTypeId = auditEventTypes[1].Id,
        //        Parameters = new List<AuditEventParameter>()
        //{
        //    new AuditEventParameter()
        //    {
        //        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.PropertyOwnerId).Id,
        //        Value = propertyOwners[0].Id
        //    },
        //     new AuditEventParameter()
        //    {
        //        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.DocumentId).Id,
        //        Value = documents[0].Id
        //    }
        //}
        //    };
        //    await auditEventService.AddAsync(auditEvent2);

        //    var auditEvent3 = new AuditEvent()
        //    {
        //        CreatedDateTime = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromMinutes(90)),
        //        EventTypeId = auditEventTypes[2].Id,
        //        Parameters = new List<AuditEventParameter>()
        //{
        //    new AuditEventParameter()
        //    {
        //        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.PropertyGroupId).Id,
        //        Value = propertyGroups[0].Id
        //    },
        //     new AuditEventParameter()
        //    {
        //        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.DocumentId).Id,
        //        Value = documents[0].Id
        //    }
        //}
        //    };
        //    await auditEventService.AddAsync(auditEvent3);

        //    var auditEvent4 = new AuditEvent()
        //    {
        //        CreatedDateTime = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromMinutes(90)),
        //        EventTypeId = auditEventTypes[2].Id,
        //        Parameters = new List<AuditEventParameter>()
        //{
        //    new AuditEventParameter()
        //    {
        //        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.IssueId).Id,
        //        Value = issues[0].Id
        //    },
        //     new AuditEventParameter()
        //    {
        //        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.DocumentId).Id,
        //        Value = documents[0].Id
        //    }
        //}
        //    };
        //    await auditEventService.AddAsync(auditEvent4);


            return Task.FromResult(entities);
        }

        public Task WriteAllAsync(List<AuditEvent> entities)
        {
            return Task.CompletedTask;
        }
    }
}

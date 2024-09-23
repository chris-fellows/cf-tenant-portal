using CFTenantPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Models
{
    public class SharedSeed
    {
        public IEntityList<AccountTransaction> AccountTransactions { get; set; }
        public IEntityList<AccountTransactionType> AccountTransactionTypes { get; set; }

        public IEntityList<AuditEvent> AuditEvents { get; set; }

        public IEntityList<AuditEventType> AuditEventTypes { get; set; }

        public IEntityList<Document> Documents { get; set; }

        public IEntityList<Employee> Employees { get; set; }

        public IEntityList<Issue> Issues { get; set; }

        public IEntityList<IssueStatus> IssueStatuses { get; set; }

        public IEntityList<IssueType> IssueTypes { get; set; }

        public IEntityList<IssueType> entityList { get; set; }

        public IEntityList<Message> Messages { get; set; }

        public IEntityList<MessageTemplate> MessageTemplates { get; set; }
        public IEntityList<MessageType> MessageTypes { get; set; }

        public IEntityList<Property> Properties { get; set; }

        public IEntityList<PropertyFeatureType> PropertyFeatureTypes { get; set; }

        public IEntityList<PropertyGroup> PropertyGroups { get; set; }

        public IEntityList<PropertyOwner> PropertyOwners { get; set; }

        public IEntityList<SystemValueType> SystemValueTypes { get; set; }

        public IEntityList<User> Users { get; set; }    
    }
}

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
        public IEntityReader<AccountTransaction> AccountTransactions { get; set; }
        public IEntityReader<AccountTransactionType> AccountTransactionTypes { get; set; }

        public IEntityReader<AuditEvent> AuditEvents { get; set; }

        public IEntityReader<AuditEventType> AuditEventTypes { get; set; }

        public IEntityReader<Document> Documents { get; set; }

        public IEntityReader<Employee> Employees { get; set; }

        public IEntityReader<Issue> Issues { get; set; }

        public IEntityReader<IssueStatus> IssueStatuses { get; set; }

        public IEntityReader<IssueType> IssueTypes { get; set; }

        public IEntityReader<IssueType> entityList { get; set; }

        public IEntityReader<Message> Messages { get; set; }

        public IEntityReader<MessageTemplate> MessageTemplates { get; set; }
        public IEntityReader<MessageType> MessageTypes { get; set; }

        public IEntityReader<Property> Properties { get; set; }

        public IEntityReader<PropertyFeatureType> PropertyFeatureTypes { get; set; }

        public IEntityReader<PropertyGroup> PropertyGroups { get; set; }

        public IEntityReader<PropertyOwner> PropertyOwners { get; set; }

        public IEntityReader<SystemValueType> SystemValueTypes { get; set; }

        public IEntityReader<User> Users { get; set; }    
    }
}

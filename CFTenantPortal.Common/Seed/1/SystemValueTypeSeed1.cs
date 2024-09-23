using CFTenantPortal.Enums;
using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;

namespace CFTenantPortal.Seed1
{
    public class SystemValueTypeSeed1 : IEntityList<SystemValueType>
    {
        public Task<List<SystemValueType>> ReadAllAsync()
        {
            var entities = new List<SystemValueType>();

            entities.Add(new SystemValueType()
            {                
                Description = "Account Transaction Id",
                ValueType = SystemValueTypes.AccountTransactionId
            });

            entities.Add(new SystemValueType()
            {             
                Description = "Document Id",
                ValueType = SystemValueTypes.DocumentId
            });

            entities.Add(new SystemValueType()
            {            
                Description = "Employee Id",
                ValueType = SystemValueTypes.DocumentId
            });

            entities.Add(new SystemValueType()
            {             
                Description = "Issue Id",
                ValueType = SystemValueTypes.IssueId
            });

            entities.Add(new SystemValueType()
            {             
                Description = "Message Id",
                ValueType = SystemValueTypes.MessageId
            });

            entities.Add(new SystemValueType()
            {             
                Description = "Message Type Id",
                ValueType = SystemValueTypes.MessageTypeId
            });

            entities.Add(new SystemValueType()
            {             
                Description = "Property Group Id",
                ValueType = SystemValueTypes.PropertyGroupId
            });

            entities.Add(new SystemValueType()
            {             
                Description = "Property Id",
                ValueType = SystemValueTypes.PropertyId
            });

            entities.Add(new SystemValueType()
            {             
                Description = "Property Owner Id",
                ValueType = SystemValueTypes.PropertyOwnerId
            });

            entities.Add(new SystemValueType()
            {             
                Description = "User Id",
                ValueType = SystemValueTypes.UserId
            });

            return Task.FromResult(entities);
        }

        public Task WriteAllAsync(List<SystemValueType> entities)
        {
            return Task.CompletedTask;
        }
    }
}

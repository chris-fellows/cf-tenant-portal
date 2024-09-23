using CFTenantPortal.Enums;
using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using MongoDB.Driver;

namespace CFTenantPortal.Services
{
    public class MongoDBSystemValueTypeService : MongoDBBaseService<SystemValueType>, ISystemValueTypeService
    {
        public MongoDBSystemValueTypeService(IDatabaseConfig databaseConfig) : base(databaseConfig, "system_value_types")
        {

        }

        //public Task<List<AccountTransaction>> GetAll()
        //{
        //    return Task.FromResult(GetAllInternal());
        //}

        //public Task<AccountTransaction> GetById(string id)
        //{
        //    return Task.FromResult(GetAllInternal().FirstOrDefault(e => e.Id == id));
        //}


        public Task<SystemValueType?> GetByIdAsync(string id)
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

        //public Task<List<SystemValueType>> GetAll()
        //{
        //    return Task.FromResult(GetAllInternal());
        //}

        //public Task<SystemValueType> GetById(string id)
        //{
        //    return Task.FromResult(GetAllInternal().FirstOrDefault(e => e.Id == id));
        //}

        public Task<SystemValueType> GetByEnum(SystemValueTypes systemValueType)
        {
            return Task.FromResult(GetAll().FirstOrDefault(e => e.ValueType == systemValueType));
        }

        //private List<SystemValueType> GetAllInternal()
        //{
        //    var systemValueTypes = new List<SystemValueType>();

        //    systemValueTypes.Add(new SystemValueType()
        //    {
        //        Id = "1",
        //        Description = "Account Transaction Id",
        //        ValueType = SystemValueTypes.AccountTransactionId
        //    });

        //    systemValueTypes.Add(new SystemValueType()
        //    {
        //        Id = "1",
        //        Description = "Document Id",
        //        ValueType = SystemValueTypes.DocumentId
        //    });

        //    systemValueTypes.Add(new SystemValueType()
        //    {
        //        Id = "1",
        //        Description = "Employee Id",
        //        ValueType = SystemValueTypes.DocumentId
        //    });

        //    systemValueTypes.Add(new SystemValueType()
        //    {
        //        Id = "1",
        //        Description = "Issue Id",
        //        ValueType = SystemValueTypes.IssueId
        //    });

        //    systemValueTypes.Add(new SystemValueType()
        //    {
        //        Id = "1",
        //        Description = "Message Id",
        //        ValueType = SystemValueTypes.MessageId
        //    });

        //    systemValueTypes.Add(new SystemValueType()
        //    {
        //        Id = "1",
        //        Description = "Message Type Id",
        //        ValueType = SystemValueTypes.MessageTypeId
        //    });

        //    systemValueTypes.Add(new SystemValueType()
        //    {
        //        Id = "1",
        //        Description = "Property Group Id",
        //        ValueType = SystemValueTypes.PropertyGroupId
        //    });

        //    systemValueTypes.Add(new SystemValueType()
        //    {
        //        Id = "1",
        //        Description = "Property Id",
        //        ValueType = SystemValueTypes.PropertyId
        //    });

        //    systemValueTypes.Add(new SystemValueType()
        //    {
        //        Id = "1",
        //        Description = "Property Owner Id",
        //        ValueType = SystemValueTypes.PropertyOwnerId
        //    });

        //    systemValueTypes.Add(new SystemValueType()
        //    {
        //        Id = "1",
        //        Description = "User Id",
        //        ValueType = SystemValueTypes.UserId
        //    });

        //    return systemValueTypes;
        //}
    }
}

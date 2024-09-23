using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using MongoDB.Driver;
using System.Net;

namespace CFTenantPortal.Services
{
    public class MongoDBMessageTypeService : MongoDBBaseService<MessageType>, IMessageTypeService
    {
        public MongoDBMessageTypeService(IDatabaseConfig databaseConfig) : base(databaseConfig, "message_types")
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


        public Task<MessageType?> GetByIdAsync(string id)
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

        //public Task<List<MessageType>> GetAll()
        //{
        //    return Task.FromResult(GetAllInternal());
        //}

        //public Task<MessageType> GetById(string id)
        //{
        //    return Task.FromResult(GetAllInternal().FirstOrDefault(it => it.Id == id));
        //}

        //public Task Update(MessageType messageType)
        //{
        //    return Task.CompletedTask;
        //}

        //private List<MessageType> GetAllInternal()
        //{
        //    var messageTypes = new List<MessageType>();

        //    messageTypes.Add(new MessageType()
        //    {
        //        Id = "1",
        //        Description = "Message type 1",
        //        DefaultTemplateId = "1"
        //    });

        //    messageTypes.Add(new MessageType()
        //    {
        //        Id = "2",
        //        Description = "Message type 2",
        //        DefaultTemplateId = "2"
        //    });

        //    messageTypes.Add(new MessageType()
        //    {
        //        Id = "3",
        //        Description = "Message type 3",
        //        DefaultTemplateId = "2"
        //    });

        //    messageTypes.Add(new MessageType()
        //    {
        //        Id = "4",
        //        Description = "Message type 4",
        //        DefaultTemplateId = "3"
        //    });

        //    messageTypes.Add(new MessageType()
        //    {
        //        Id = "5",
        //        Description = "Message type 5",
        //        DefaultTemplateId = "4"
        //    });

        //    messageTypes.Add(new MessageType()
        //    {
        //        Id = "6",
        //        Description = "Issue completed",
        //        DefaultTemplateId = "4"
        //    });

        //    return messageTypes;
        //}
    }
}

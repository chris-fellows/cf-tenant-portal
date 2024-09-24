using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using MongoDB.Driver;

namespace CFTenantPortal.Services
{
    public class MongoDBMessageService : MongoDBBaseService<Message>, IMessageService
    {
        public MongoDBMessageService(IDatabaseConfig databaseConfig) : base(databaseConfig, "messages")
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


        public Task<Message?> GetByIdAsync(string id)
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

        //public Task<List<Message>> GetAll()
        //{
        //    return Task.FromResult(GetAllInternal());
        //}

        //public Task<Message> GetById(string id)
        //{
        //    return Task.FromResult(GetAllInternal().FirstOrDefault(it => it.Id == id));
        //}

        public Task<List<Message>> GetByPropertyOwner(string propertyOwnerId)
        {
            return Task.FromResult(GetAll().Where(m => m.PropertyOwnerId == propertyOwnerId).ToList());
        }

        public Task<List<Message>> GetByIssue(string issueId)
        {
            return Task.FromResult(GetAll().Where(m => m.IssueId == issueId).ToList());
        }

        public Task<List<Message>> GetByProperty(string propertyId)
        {
            return Task.FromResult(GetAll().Where(m => m.PropertyId == propertyId).ToList());
        }

        //public Task Update(Message message)
        //{
        //    return Task.CompletedTask;
        //}

        //private List<Message> GetAllInternal()
        //{
        //    var messages = new List<Message>();

        //    messages.Add(new Message()
        //    {
        //        Id = "1",
        //        CreatedDateTime = DateTimeOffset.Now,
        //        MessageTypeId = "1",
        //        PropertyOwnerId = "1", 
        //        Text = "Test message",
        //        DocumentIds = new List<string>() { "1" }
        //    });

        //    messages.Add(new Message()
        //    {
        //        Id = "2",
        //        CreatedDateTime = DateTimeOffset.Now,
        //        MessageTypeId = "2",
        //        PropertyOwnerId = "1",
        //        Text = "Test message",
        //    });

        //    messages.Add(new Message()
        //    {
        //        Id = "3",
        //        CreatedDateTime = DateTimeOffset.Now,
        //        MessageTypeId = "2",
        //        PropertyOwnerId = "3",
        //        Text = "Test message",
        //        DocumentIds = new List<string>() { "2", "3" }
        //    });

        //    messages.Add(new Message()
        //    {
        //        Id = "4",
        //        CreatedDateTime = DateTimeOffset.Now,
        //        MessageTypeId = "2",
        //        PropertyOwnerId = "4",
        //        Text = "Test message"
        //    });

        //    return messages;
        //}
    }
}

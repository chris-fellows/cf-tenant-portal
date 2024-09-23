using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using MongoDB.Driver;

namespace CFTenantPortal.Services
{
    public class MongoDBMessageTemplateService : MongoDBBaseService<MessageTemplate>, IMessageTemplateService
    {

        public MongoDBMessageTemplateService(IDatabaseConfig databaseConfig) : base(databaseConfig, "message_templates")
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


        public Task<MessageTemplate?> GetByIdAsync(string id)
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

        //public Task<List<MessageTemplate>> GetAll()
        //{
        //    return Task.FromResult(GetAllInternal());
        //}

        //public Task<MessageTemplate> GetById(string id)
        //{
        //    return Task.FromResult(GetAllInternal().FirstOrDefault(it => it.Id == id));
        //}

        //public Task Update(MessageTemplate messageTemplate)
        //{
        //    return Task.CompletedTask;
        //}

        //private List<MessageTemplate> GetAllInternal()
        //{
        //    var messageTemplates = new List<MessageTemplate>();

        //    messageTemplates.Add(new MessageTemplate()
        //    {
        //        Id = "1",
        //        MessageTypeId = "1",
        //        Text = "This is a template"                
        //    });

        //    messageTemplates.Add(new MessageTemplate()
        //    {
        //        Id = "2",
        //        MessageTypeId = "1",
        //        Text = "This is a template"
        //    });

        //    messageTemplates.Add(new MessageTemplate()
        //    {
        //        Id = "3",
        //        MessageTypeId = "1",
        //        Text = "This is a template"
        //    });

        //    messageTemplates.Add(new MessageTemplate()
        //    {
        //        Id = "4",
        //        MessageTypeId = "1",
        //        Text = "This is a template"
        //    });

        //    messageTemplates.Add(new MessageTemplate()
        //    {
        //        Id = "5",
        //        MessageTypeId = "1",
        //        Text = "This is a template"
        //    });

        //    return messageTemplates;
        //}
    }
}

using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;

namespace CFTenantPortal.Services
{
    public class MessageTemplateService : IMessageTemplateService
    {
        public Task<List<MessageTemplate>> GetAll()
        {
            return Task.FromResult(GetAllInternal());
        }

        public Task<MessageTemplate> GetById(string id)
        {
            return Task.FromResult(GetAllInternal().FirstOrDefault(it => it.Id == id));
        }

        public Task Update(MessageTemplate messageTemplate)
        {
            return Task.CompletedTask;
        }

        private List<MessageTemplate> GetAllInternal()
        {
            var messageTemplates = new List<MessageTemplate>();

            messageTemplates.Add(new MessageTemplate()
            {
                Id = "1",
                MessageTypeId = "1",
                Text = "This is a template"                
            });

            messageTemplates.Add(new MessageTemplate()
            {
                Id = "2",
                MessageTypeId = "1",
                Text = "This is a template"
            });

            messageTemplates.Add(new MessageTemplate()
            {
                Id = "3",
                MessageTypeId = "1",
                Text = "This is a template"
            });

            messageTemplates.Add(new MessageTemplate()
            {
                Id = "4",
                MessageTypeId = "1",
                Text = "This is a template"
            });

            messageTemplates.Add(new MessageTemplate()
            {
                Id = "5",
                MessageTypeId = "1",
                Text = "This is a template"
            });

            return messageTemplates;
        }
    }
}

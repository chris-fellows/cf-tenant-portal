using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;

namespace CFTenantPortal.Services
{
    public class MessageService : IMessageService
    {
        public Task<List<Message>> GetAll()
        {
            return Task.FromResult(GetAllInternal());
        }

        public Task<Message> GetById(string id)
        {
            return Task.FromResult(GetAllInternal().FirstOrDefault(it => it.Id == id));
        }

        public Task<List<Message>> GetByPropertyOwner(string propertyOwnerId)
        {
            return Task.FromResult(GetAllInternal().Where(m => m.PropertyOwnerId == propertyOwnerId).ToList());
        }

        public Task Update(Message message)
        {
            return Task.CompletedTask;
        }

        private List<Message> GetAllInternal()
        {
            var messages = new List<Message>();

            messages.Add(new Message()
            {
                Id = "1",
                CreatedDateTime = DateTimeOffset.Now,
                MessageTypeId = "1",
                PropertyOwnerId = "1", 
                Text = "Test message"                
            });

            messages.Add(new Message()
            {
                Id = "2",
                CreatedDateTime = DateTimeOffset.Now,
                MessageTypeId = "2",
                PropertyOwnerId = "1",
                Text = "Test message"
            });

            messages.Add(new Message()
            {
                Id = "3",
                CreatedDateTime = DateTimeOffset.Now,
                MessageTypeId = "2",
                PropertyOwnerId = "3",
                Text = "Test message"
            });

            messages.Add(new Message()
            {
                Id = "4",
                CreatedDateTime = DateTimeOffset.Now,
                MessageTypeId = "2",
                PropertyOwnerId = "4",
                Text = "Test message"
            });

            return messages;
        }
    }
}

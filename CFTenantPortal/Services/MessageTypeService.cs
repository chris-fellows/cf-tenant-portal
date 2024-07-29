using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using System.Net;

namespace CFTenantPortal.Services
{
    public class MessageTypeService : IMessageTypeService
    {
        public Task<List<MessageType>> GetAll()
        {
            return Task.FromResult(GetAllInternal());
        }

        public Task<MessageType> GetById(string id)
        {
            return Task.FromResult(GetAllInternal().FirstOrDefault(it => it.Id == id));
        }

        public Task Update(MessageType messageType)
        {
            return Task.CompletedTask;
        }

        private List<MessageType> GetAllInternal()
        {
            var messageTypes = new List<MessageType>();

            messageTypes.Add(new MessageType()
            {
                Id = "1",
                Description = "Message type 1"
            });

            messageTypes.Add(new MessageType()
            {
                Id = "2",
                Description = "Message type 2"
            });

            messageTypes.Add(new MessageType()
            {
                Id = "3",
                Description = "Message type 3"
            });

            messageTypes.Add(new MessageType()
            {
                Id = "4",
                Description = "Message type 4"
            });

            messageTypes.Add(new MessageType()
            {
                Id = "5",
                Description = "Message type 5"
            });

            return messageTypes;
        }
    }
}

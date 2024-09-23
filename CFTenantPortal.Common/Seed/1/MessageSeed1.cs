using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Seed1
{
    public class MessageSeed1 : IEntityList<Message>
    {
        private readonly IDocumentService _documentService;
        private readonly IMessageTypeService _messageTypeService;
        private readonly IPropertyOwnerService _propertyOwnerService;

        public MessageSeed1(IDocumentService documentService,
                            IMessageTypeService messageTypeService,
                            IPropertyOwnerService propertyOwnerService)
        {
            _documentService = documentService;
            _messageTypeService = messageTypeService;
            _propertyOwnerService = propertyOwnerService;
        }

        public Task<List<Message>> ReadAllAsync()
        {
            var entities = new List<Message>();

            var documents = _documentService.GetAll().ToList();
            var messageTypes = _messageTypeService.GetAll().ToList();
            var propertyOwners = _propertyOwnerService.GetAll().ToList();

            var document1 = documents[0];
            var document2 = documents[1];
            var document3 = documents[2];
            var document4 = documents[3];

            var messageType1 = messageTypes[0];
            var messageType2 = messageTypes[1];
            var messageType3 = messageTypes[2];

            var propertyOwner1 = propertyOwners[0];
            var propertyOwner2 = propertyOwners[1];
            var propertyOwner3 = propertyOwners[3];
            var propertyOwner4 = propertyOwners[4];

            entities.Add(new Message()
            {                
                CreatedDateTime = DateTimeOffset.Now,
                MessageTypeId = messageType1.Id,
                PropertyOwnerId = propertyOwner1.Id,
                Text = "Test message",
                DocumentIds = new List<string>() { document1.Id }
            });

            entities.Add(new Message()
            {             
                CreatedDateTime = DateTimeOffset.Now,
                MessageTypeId = messageType2.Id,
                PropertyOwnerId = propertyOwner1.Id,
                Text = "Test message",
            });

            entities.Add(new Message()
            {             
                CreatedDateTime = DateTimeOffset.Now,
                MessageTypeId = messageType2.Id,
                PropertyOwnerId = propertyOwner3.Id,
                Text = "Test message",
                DocumentIds = new List<string>() { document2.Id, document3.Id }
            });

            entities.Add(new Message()
            {             
                CreatedDateTime = DateTimeOffset.Now,
                MessageTypeId = messageType2.Id,
                PropertyOwnerId = propertyOwner4.Id,
                Text = "Test message"
            });

            return Task.FromResult(entities);
        }

        public Task WriteAllAsync(List<Message> entities)
        {
            return Task.CompletedTask;
        }
    }
}

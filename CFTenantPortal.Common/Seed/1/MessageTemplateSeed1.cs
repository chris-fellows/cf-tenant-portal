using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Seed1
{
    public class MessageTemplateSeed1 : IEntityList<MessageTemplate>
    {
        private readonly IMessageTypeService _messageTypeService;

        public MessageTemplateSeed1(IMessageTypeService messageTypeService)
        {
            _messageTypeService = messageTypeService;

        }

        public Task<List<MessageTemplate>> ReadAllAsync()
        {
            var entities = new List<MessageTemplate>();

            var messageTypes = _messageTypeService.GetAll().ToList();

            var messageType1 = messageTypes[0];
            var messageType2 = messageTypes[1];
            var messageType3 = messageTypes[2];

            entities.Add(new MessageTemplate()
            {                
                MessageTypeId = messageType1.Id,
                Text = "This is a template"
            });

            entities.Add(new MessageTemplate()
            {
                MessageTypeId = messageType1.Id,
                Text = "This is a template"
            });

            entities.Add(new MessageTemplate()
            {
                MessageTypeId = messageType1.Id,
                Text = "This is a template"
            });

            entities.Add(new MessageTemplate()
            {
                MessageTypeId = messageType1.Id,
                Text = "This is a template"
            });

            entities.Add(new MessageTemplate()
            {
                MessageTypeId = messageType2.Id,
                Text = "This is a template"
            });

            return Task.FromResult(entities);
        }

        public Task WriteAllAsync(List<MessageTemplate> entities)
        {
            return Task.CompletedTask;
        }
    }
}

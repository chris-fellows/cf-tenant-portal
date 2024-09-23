using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Seed1
{
    public class MessageTypeSeed1 : IEntityList<MessageType>
    {
        public Task<List<MessageType>> ReadAllAsync()
        {
            var entities = new List<MessageType>();

            entities.Add(new MessageType()
            {                
                Description = "Message type 1",
                DefaultTemplateId = ""
            });

            entities.Add(new MessageType()
            {             
                Description = "Message type 2",
                DefaultTemplateId = ""
            });

            entities.Add(new MessageType()
            {             
                Description = "Message type 3",
                DefaultTemplateId = ""
            });

            entities.Add(new MessageType()
            {             
                Description = "Message type 4",
                DefaultTemplateId = ""
            });

            entities.Add(new MessageType()
            {             
                Description = "Message type 5",
                DefaultTemplateId = ""
            });

            entities.Add(new MessageType()
            {             
                Description = "Issue completed",
                DefaultTemplateId = ""
            });

            return Task.FromResult(entities);
        }

        public Task WriteAllAsync(List<MessageType> entities)
        {
            return Task.CompletedTask;
        }
    }
}

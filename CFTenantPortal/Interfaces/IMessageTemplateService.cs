using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IMessageTemplateService
    {
        Task<List<MessageTemplate>> GetAll();

        Task<MessageTemplate> GetById(string id);

        Task Update(MessageTemplate messageTemplate);
    }
}

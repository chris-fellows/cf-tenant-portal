using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IMessageTypeService
    {
        Task<List<MessageType>> GetAll();

        Task<MessageType> GetById(string id);

        Task Update(MessageType messageType);
    }
}

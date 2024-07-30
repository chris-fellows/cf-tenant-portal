using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IMessageService
    {
        Task<List<Message>> GetAll();

        Task<Message> GetById(string id);

        Task<List<Message>> GetByPropertyOwner(string propertyOwnerId);

        Task Update(Message message);
    }
}

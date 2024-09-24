using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IMessageService : IEntityWithIDService<Message, string>
    {
        //Task<List<Message>> GetAll();

        //Task<Message> GetById(string id);

        Task<List<Message>> GetByPropertyOwner(string propertyOwnerId);

        Task<List<Message>> GetByIssue(string issueId);

        Task<List<Message>> GetByProperty(string propertyId);

        //Task Update(Message message);
    }
}

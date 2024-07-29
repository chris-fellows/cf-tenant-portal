using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAll();

        Task<User> GetById(string id);

        Task Update(User user);
    }
}

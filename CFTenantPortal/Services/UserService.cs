using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using System.Net;

namespace CFTenantPortal.Services
{
    public class UserService : IUserService
    {
        public Task<List<User>> GetAll()
        {
            return null;
        }

        public Task<User> GetById(string id)
        {
            return null;
        }

        public Task Update(User user)
        {
            return Task.CompletedTask;
        }
    }
}

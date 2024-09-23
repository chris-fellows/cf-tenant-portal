using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using MongoDB.Driver;
using System.Net;

namespace CFTenantPortal.Services
{
    public class MongoDBUserService : MongoDBBaseService<User>, IUserService
    {
        public MongoDBUserService(IDatabaseConfig databaseConfig) : base(databaseConfig, "users")
        {

        }

        //public Task<List<AccountTransaction>> GetAll()
        //{
        //    return Task.FromResult(GetAllInternal());
        //}

        //public Task<AccountTransaction> GetById(string id)
        //{
        //    return Task.FromResult(GetAllInternal().FirstOrDefault(e => e.Id == id));
        //}


        public Task<User?> GetByIdAsync(string id)
        {
            return _entities.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        //public Task<AccountTransaction?> GetByNameAsync(string name)
        //{
        //    return _entities.Find(x => x.Name == name).FirstOrDefaultAsync();
        //}

        public Task DeleteByIdAsync(string id)
        {
            return _entities.DeleteOneAsync(id);
        }

        //public Task<List<User>> GetAll()
        //{
        //    return null;
        //}

        //public Task<User> GetById(string id)
        //{
        //    return null;
        //}

        //public Task Update(User user)
        //{
        //    return Task.CompletedTask;
        //}
    }
}

using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using MongoDB.Driver;

namespace CFTenantPortal.Services
{
    public class MongoDBAccountTransactionService : MongoDBBaseService<AccountTransaction>, IAccountTransactionService
    {
        public MongoDBAccountTransactionService(IDatabaseConfig databaseConfig) : base(databaseConfig, "account_transactions")
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


        public Task<AccountTransaction?> GetByIdAsync(string id)
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

        public Task<List<AccountTransaction>> GetByProperty(string propertyId)
        {
            return Task.FromResult(GetAll().Where(at => at.PropertyId == propertyId).ToList());
        }

        //public Task Update(AccountTransaction accountTransaction)
        //{
        //    return Task.CompletedTask;
        //}

        //private List<AccountTransaction> GetAllInternal()
        //{
        //    var accountTransactions = new List<AccountTransaction>();

        //    accountTransactions.Add(new AccountTransaction()
        //    {
        //        Id = "1",
        //        PropertyId = "1",
        //        Reference = Guid.NewGuid().ToString(),
        //        TypeId = "5",       // Rollup
        //        Value = 10.00
        //    });

        //    accountTransactions.Add(new AccountTransaction()
        //    {
        //        Id = "2",
        //        PropertyId = "2",
        //        Reference = Guid.NewGuid().ToString(),
        //        TypeId = "5",
        //        Value = 10.00
        //    });

        //    accountTransactions.Add(new AccountTransaction()
        //    {
        //        Id = "3",
        //        PropertyId = "3",
        //        Reference = Guid.NewGuid().ToString(),
        //        TypeId = "5",
        //        Value = 10.00
        //    });

        //    accountTransactions.Add(new AccountTransaction()
        //    {
        //        Id = "4",
        //        PropertyId = "4",
        //        Reference = Guid.NewGuid().ToString(),
        //        TypeId = "5",
        //        Value = 10.00
        //    });

        //    accountTransactions.Add(new AccountTransaction()
        //    {
        //        Id = "5",
        //        PropertyId = "5",
        //        Reference = Guid.NewGuid().ToString(),
        //        TypeId = "5",
        //        Value = 10.00
        //    });

        //    accountTransactions.Add(new AccountTransaction()
        //    {
        //        Id = "6",
        //        PropertyId = "1",
        //        Reference = Guid.NewGuid().ToString(),
        //        TypeId = "1",
        //        Value = 540.00
        //    });

        //    return accountTransactions;
        //}
    }
}

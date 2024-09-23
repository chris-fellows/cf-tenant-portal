using CFTenantPortal.Enums;
using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using MongoDB.Driver;

namespace CFTenantPortal.Services
{
    public class MongoDBAccountTransactionTypeService :  MongoDBBaseService<AccountTransactionType>, IAccountTransactionTypeService
    {
        public MongoDBAccountTransactionTypeService(IDatabaseConfig databaseConfig) : base(databaseConfig, "account_transaction_types")
        {

        }

        public Task<AccountTransactionType?> GetByIdAsync(string id)
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

        //public Task<List<AccountTransactionType>> GetAll()
        //{
        //    return Task.FromResult(GetAllInternal());
        //}

        //public Task<AccountTransactionType> GetById(string id)
        //{
        //    return Task.FromResult(GetAllInternal().FirstOrDefault(e => e.Id == id));
        //}

        //public Task Update(AccountTransactionType accountTransactionType)
        //{
        //    return Task.CompletedTask;
        //}

        //private List<AccountTransactionType> GetAllInternal()
        //{
        //    var accountTransactionTypes = new List<AccountTransactionType>();

        //    accountTransactionTypes.Add(new AccountTransactionType()
        //    {
        //        Id = "1",                
        //        Description = "Management fees request"               
        //    });

        //    accountTransactionTypes.Add(new AccountTransactionType()
        //    {
        //        Id = "2",
        //        Description = "Management fees payment"
        //    });

        //    accountTransactionTypes.Add(new AccountTransactionType()
        //    {
        //        Id = "3",
        //        Description = "Ground rent request"
        //    });

        //    accountTransactionTypes.Add(new AccountTransactionType()
        //    {
        //        Id = "4",
        //        Description = "Ground rent payment"
        //    });

        //    // Prior to deletion then old transaction are rolled up in to a single transaction
        //    accountTransactionTypes.Add(new AccountTransactionType()
        //    {
        //        Id = "5",
        //        Description = "Opening transaction (Rollup)",
        //        TransactionType = AccountTransactionTypes.Rollup
        //    });

        //    return accountTransactionTypes;
        //}
    }
}

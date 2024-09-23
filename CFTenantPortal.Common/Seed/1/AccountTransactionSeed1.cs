using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Seed1
{
    public class AccountTransactionSeed1 : IEntityList<AccountTransaction>
    {
        private readonly IAccountTransactionTypeService _accountTransactionTypeService;
        private readonly IPropertyService _propertyService;

        public AccountTransactionSeed1(IAccountTransactionTypeService accountTransactionTypeService,
                                    IPropertyService propertyService)
        {
            _accountTransactionTypeService = accountTransactionTypeService;
            _propertyService = propertyService;
        }

        public Task<List<AccountTransaction>> ReadAllAsync()
        {
            var entities = new List<AccountTransaction>();

            var accountTransactionTypes = _accountTransactionTypeService.GetAll().ToList();
            var properties = _propertyService.GetAll().ToList();

            var rollupTrans = accountTransactionTypes.First(t => t.TransactionType == Enums.AccountTransactionTypes.Rollup);
            var mgtFeesRequest = accountTransactionTypes.First(t => t.Description.Equals("Management fees request"));

            var property1 = properties[0];
            var property2 = properties[1];

            entities.Add(new AccountTransaction()
            {                
                PropertyId = property1.Id,
                Reference = Guid.NewGuid().ToString(),
                TypeId = rollupTrans.Id,       // Rollup
                Value = 10.00
            });

            entities.Add(new AccountTransaction()
            {
                PropertyId = property2.Id,
                Reference = Guid.NewGuid().ToString(),
                TypeId = rollupTrans.Id,       // Rollup
                Value = 10.00
            });

            entities.Add(new AccountTransaction()
            {
                PropertyId = property1.Id,
                Reference = Guid.NewGuid().ToString(),
                TypeId = rollupTrans.Id,       // Rollup
                Value = 10.00
            });

            entities.Add(new AccountTransaction()
            {
                PropertyId = property1.Id,
                Reference = Guid.NewGuid().ToString(),
                TypeId = rollupTrans.Id,       // Rollup
                Value = 10.00
            });

            entities.Add(new AccountTransaction()
            {
                PropertyId = property1.Id,
                Reference = Guid.NewGuid().ToString(),
                TypeId = rollupTrans.Id,       // Rollup
                Value = 10.00
            });

            entities.Add(new AccountTransaction()
            {
                PropertyId = property2.Id,
                Reference = Guid.NewGuid().ToString(),
                TypeId = mgtFeesRequest.Id,
                Value = 540.00
            });

            return Task.FromResult(entities);
        }

        public Task WriteAllAsync(List<AccountTransaction> entities)
        {
            return Task.CompletedTask;
        }
    }
}

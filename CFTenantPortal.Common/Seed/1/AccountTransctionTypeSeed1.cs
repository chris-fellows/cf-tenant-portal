using CFTenantPortal.Enums;
using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Seed1
{
    public class AccountTransactionTypeSeed1 : IEntityList<AccountTransactionType>
    {
        public Task<List<AccountTransactionType>> ReadAllAsync()
        {
            var entities = new List<AccountTransactionType>();

            entities.Add(new AccountTransactionType()
              {                  
                  Description = "Management fees request"
              });

            entities.Add(new AccountTransactionType()
            {               
                Description = "Management fees payment"
            });

            entities.Add(new AccountTransactionType()
            {             
                Description = "Ground rent request"
            });

            entities.Add(new AccountTransactionType()
            {             
                Description = "Ground rent payment"                
            });

            // Prior to deletion then old transaction are rolled up in to a single transaction
            entities.Add(new AccountTransactionType()
            {             
                Description = "Opening transaction (Rollup)",
                TransactionType = AccountTransactionTypes.Rollup
            });

            return Task.FromResult(entities);
        }

        public Task WriteAllAsync(List<AccountTransactionType> entities)
        {
            return Task.CompletedTask;
        }
    }
}

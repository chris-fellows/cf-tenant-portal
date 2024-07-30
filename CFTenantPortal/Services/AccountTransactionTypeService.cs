﻿using CFTenantPortal.Constants;
using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;

namespace CFTenantPortal.Services
{
    public class AccountTransactionTypeService : IAccountTransactionTypeService
    {
        public Task<List<AccountTransactionType>> GetAll()
        {
            return Task.FromResult(GetAllInternal());
        }

        public Task<AccountTransactionType> GetById(string id)
        {
            return Task.FromResult(GetAllInternal().FirstOrDefault(e => e.Id == id));
        }

        public Task Update(AccountTransactionType accountTransactionType)
        {
            return Task.CompletedTask;
        }

        private List<AccountTransactionType> GetAllInternal()
        {
            var accountTransactionTypes = new List<AccountTransactionType>();

            accountTransactionTypes.Add(new AccountTransactionType()
            {
                Id = "1",                
                Description = "Management fees request"               
            });

            accountTransactionTypes.Add(new AccountTransactionType()
            {
                Id = "2",
                Description = "Management fees payment"
            });

            accountTransactionTypes.Add(new AccountTransactionType()
            {
                Id = "3",
                Description = "Ground rent request"
            });

            accountTransactionTypes.Add(new AccountTransactionType()
            {
                Id = "4",
                Description = "Ground rent payment"
            });

            // Prior to deletion then old transaction are rolled up in to a single transaction
            accountTransactionTypes.Add(new AccountTransactionType()
            {
                Id = "5",
                Description = "Opening transaction (Rollup)",
                InternalName = AccountTransactionTypeInternalNames.Rollup
            });

            return accountTransactionTypes;
        }
    }
}

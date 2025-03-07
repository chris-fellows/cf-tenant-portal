using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Services
{
    public class MongoDBBankPaymentService : MongoDBBaseService<BankPayment>, IBankPaymentService
    {
        public MongoDBBankPaymentService(IDatabaseConfig databaseConfig) : base(databaseConfig, "bank_payments")
        {

        }

        public async Task<BankPayment?> GetByIdAsync(string id)
        {
            return await _entities.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}

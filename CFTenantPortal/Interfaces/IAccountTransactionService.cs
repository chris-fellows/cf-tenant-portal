using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IAccountTransactionService
    {
        Task<List<AccountTransaction>> GetAll();

        Task<AccountTransaction> GetById(string id);

        Task<List<AccountTransaction>> GetByProperty(string propertyId);

        Task Update(AccountTransaction accountTransaction);
    }
}

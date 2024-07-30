using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IAccountTransactionTypeService
    {
        Task<List<AccountTransactionType>> GetAll();

        Task<AccountTransactionType> GetById(string id);

        Task Update(AccountTransactionType accountTransactionType);
    }
}

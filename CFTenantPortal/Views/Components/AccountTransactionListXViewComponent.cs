using CFTenantPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace CFTenantPortal.Views.Components
{
    public class AccountTransactionListXViewComponent : ViewComponent
    {
        public AccountTransactionListXViewComponent()
        {
            int xxx = 1000;
        }

        public async Task<IViewComponentResult> InvokeAsync(List<AccountTransactionBasicVM> accountTransactions)
        {
            var model = new AccountTransactionListVM()
            {
                AccountBalance = accountTransactions.Sum(at => at.Value),
                AccountTransactions = accountTransactions               
            };
            return await Task.FromResult((IViewComponentResult)View("AccountTransactionListTest", model));
        }
    }
}

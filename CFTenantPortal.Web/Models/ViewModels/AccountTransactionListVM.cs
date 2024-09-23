using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    public class AccountTransactionListVM
    {
        public string HeaderText { get; set; } = String.Empty;

        [Display(Name = "Account Balance")]
        public double AccountBalance { get; set; }

        public List<AccountTransactionBasicVM> AccountTransactions { get; set; } = new List<AccountTransactionBasicVM>();
    }
}

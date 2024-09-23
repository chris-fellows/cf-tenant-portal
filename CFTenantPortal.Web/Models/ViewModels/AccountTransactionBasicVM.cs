using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    public class AccountTransactionBasicVM
    {
        public string Id { get; set; } = String.Empty;

        [Display(Name = "Reference")]
        public string Reference { get; set; } = String.Empty;

        [Display(Name = "Type")]
        public string TypeDescription { get; set; } = String.Empty;

        [Display(Name = "Value")]
        public double Value { get; set; }

        [Display(Name = "Created")]
        public DateTimeOffset CreatedDateTime { get; set; }
    }
}

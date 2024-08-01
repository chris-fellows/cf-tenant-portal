using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Property view model
    /// </summary>
    public class PropertyVM
    {
        public string Id { get; set; } = String.Empty;

        public string HeaderText = String.Empty;

        [Display(Name = "Address")]
        public AddressVM Address { get; set; }

        [Display(Name = "Property Group")]
        public string PropertyGroupId { get; set; } = String.Empty;

        [Display(Name = "Owner")]
        public string PropertyOwnerId { get; set; } = String.Empty;

        [Display(Name = "Documents")]
        public List<DocumentBasicVM> Documents { get; set; } = new List<DocumentBasicVM>();

        public List<IssueBasicVM> Issues { get; set; }

        public List<AccountTransactionBasicVM> AccountTransactions { get; set; }

        public List<EntityReference> PropertyGroupList { get; set; } = new List<EntityReference>();

        public List<EntityReference> PropertyOwnerList { get; set; } = new List<EntityReference>();
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        //public List<DocumentBasicVM> Documents { get; set; } = new List<DocumentBasicVM>();        
        public DocumentListVM DocumentList { get; set; }

        public MessageListVM MessageList { get; set; }

        [ValidateNever]
        //public List<IssueBasicVM> Issues { get; set; }
        public IssueListVM IssueList { get; set; }

        [ValidateNever]
        public List<AccountTransactionBasicVM> AccountTransactions { get; set; }

        [ValidateNever]
        public List<EntityReference> PropertyGroupRefList { get; set; } = new List<EntityReference>();

        [ValidateNever]
        public List<EntityReference> PropertyOwnerRefList { get; set; } = new List<EntityReference>();

        public bool AllowSave { get; set; }
    }
}

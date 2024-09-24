using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    public class PropertyOwnerVM
    {
        public string Id { get; set; } = String.Empty;

        public string HeaderText = String.Empty;

        [Required]
        [MaxLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; } = String.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = String.Empty;
        
        [Phone(ErrorMessage = "Phone is invalid")]
        [Display(Name = "Phone")]
        public string Phone { get; set; } = String.Empty;

        [Display(Name = "Address")]
        public AddressVM Address { get; set; }

        [ValidateNever]
        [Display(Name = "Properties")]
        //public List<PropertyBasicVM> Properties { get; set; }
        public PropertyListVM PropertyList { get; set; } = new PropertyListVM();

        [ValidateNever]
        [Display(Name = "Messages")]
        public MessageListVM MessageList { get; set; } = new MessageListVM();
        //public List<MessageBasicVM> Messages { get; set; }

        //public List<DocumentBasicVM> Documents { get; set; } = new List<DocumentBasicVM>();
        public DocumentListVM DocumentList { get; set; } = new DocumentListVM();

        public bool AllowSave { get; set; }

        //public bool AllowDelete { get; set; }
    }
}

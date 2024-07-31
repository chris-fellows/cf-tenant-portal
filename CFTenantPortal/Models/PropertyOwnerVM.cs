using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    public class PropertyOwnerVM
    { 
        public string Id { get; set; } = String.Empty;

        public string HeaderText = String.Empty;

        [Display(Name = "Name")]
        public string Name { get; set; } = String.Empty;

        [Display(Name = "Email")]
        public string Email { get; set; } = String.Empty;

        [Display(Name = "Phone")]
        public string Phone { get; set; } = String.Empty;

        [Display(Name = "Address")]
        public AddressVM Address { get; set; }

        [Display(Name = "Properties")]
        public List<PropertyBasicVM> Properties { get; set; }

        [Display(Name = "Messages")]
        public List<MessageBasicVM> Messages { get; set; }

        public List<DocumentBasicVM> Documents { get; set; } = new List<DocumentBasicVM>();
    }
}

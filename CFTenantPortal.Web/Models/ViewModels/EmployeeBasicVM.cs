using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    public class EmployeeBasicVM
    {
        public string Id { get; set; } = String.Empty;

        [MaxLength(100)]
        [Display(Name="Name")]
        public string Name { get; set; } = String.Empty;

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = String.Empty;

        [Display(Name = "Active")]
        public bool Active { get; set; }

        public bool AllowDelete { get; set; }
    }
}

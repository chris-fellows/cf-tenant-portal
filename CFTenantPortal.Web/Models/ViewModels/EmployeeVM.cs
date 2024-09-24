using CFTenantPortal.Enums;
using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Employee view model
    /// </summary>
    public class EmployeeVM
    { 
        /// <summary>
        /// Unique Id
        /// </summary>
        public string Id { get; set; } = String.Empty;

        public string HeaderText { get; set; } = String.Empty;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = String.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = String.Empty;

        [Required]
        public string Password { get; set; } = String.Empty;

        public List<UserRoles> Roles { get; set; }

        public bool Active { get; set; } = true;

        public bool AllowSave { get; set; }
    }
}

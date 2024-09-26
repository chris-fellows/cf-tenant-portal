using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    public class LoginVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = String.Empty;

        [Required]
        public string Password { get; set; } = String.Empty;
    }
}

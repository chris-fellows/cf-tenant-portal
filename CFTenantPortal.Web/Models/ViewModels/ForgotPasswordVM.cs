using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = String.Empty;
    }
}

using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    public class ResetPasswordVM
    {
        /// <summary>
        /// Id of password reset
        /// </summary>
        public string PasswordResetId { get; set; } = String.Empty;

        public string UserId { get; set; } = String.Empty;

        [Required]
        [Display(Name = "New password")]
        public string Password1 { get; set; } = String.Empty;

        [Required]
        [Display(Name = "Confirm password")]
        public string Password2 { get; set; } = String.Empty;
    }
}

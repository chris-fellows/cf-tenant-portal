using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Message basic details view model
    /// </summary>
    public class MessageBasicVM
    {
        public string Id { get; set; } = String.Empty;

        [Display(Name = "Type")]
        public string TypeDescription { get; set; } = String.Empty;

        [Display(Name = "Owner")]
        public string PropertyOwnerName { get; set; } = String.Empty;

        public string PropertyOwnerId { get; set; } = String.Empty;
    }
}

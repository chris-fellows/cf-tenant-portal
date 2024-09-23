using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Property basic details for view model
    /// </summary>
    public class PropertyBasicVM
    {
        public string Id { get; set; } = String.Empty;

        [Display(Name = "Address")]
        public string AddressDescription { get; set; } = String.Empty;

        [Display(Name = "Group")]
        public string PropertyGroupName { get; set; } = String.Empty;

        public string PropertyGroupId { get; set; } = String.Empty;

        [Display(Name = "Owner")]
        public string PropertyOwnerName { get; set; } = String.Empty;

        public string PropertyOwnerId { get; set; } = String.Empty;
    }
}

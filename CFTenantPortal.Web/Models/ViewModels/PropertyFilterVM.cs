using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    public class PropertyFilterVM
    {
        [Display(Name = "Property Group")]
        public string PropertyGroupId { get; set; } = String.Empty;

        [Display(Name = "Property Owner")]
        public string PropertyOwnerId { get; set; } = String.Empty;

        [ValidateNever]
        public List<EntityReference> PropertyGroupList { get; set; } = new List<EntityReference>();

        [ValidateNever]
        public List<EntityReference> PropertyOwnerList { get; set; } = new List<EntityReference>();
    }
}

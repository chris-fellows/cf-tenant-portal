using CFTenantPortal.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    public class AuditEventFilterVM
    {
        [Display(Name = "From Created")]
        public DateTimeOffset StartCreatedDateTime { get; set; }

        [Display(Name = "To Created")]
        public DateTimeOffset EndCreatedDateTime { get; set; }

        [Display(Name = "Event Type")]
        public string AuditEventTypeId { get; set; } = String.Empty;

        [Display(Name = "Property Group")]
        public string PropertyGroupId { get; set; } = String.Empty;

        [Display(Name = "Property")]
        public string PropertyId { set; get; } = String.Empty;

        [Display(Name = "Property Owner")]
        public string PropertyOwnerId { get; set; } = String.Empty;        

        [ValidateNever]
        public List<EntityReference> AuditEventTypeRefList { get; set; } = new List<EntityReference>();

        [ValidateNever]
        public List<EntityReference> PropertyRefList { get; set; } = new List<EntityReference>();

        [ValidateNever]
        public List<EntityReference> PropertyGroupRefList { get; set; } = new List<EntityReference>();

        [ValidateNever]
        public List<EntityReference> PropertyOwnerRefList { get; set; } = new List<EntityReference>();        
    }
}

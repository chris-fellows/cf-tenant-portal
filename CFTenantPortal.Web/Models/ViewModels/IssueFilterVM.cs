using CFTenantPortal.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    public class IssueFilterVM
    {
        [Display(Name = "Reference")]
        public string Reference { get; set; } = String.Empty;

        [Display(Name = "Status")]
        public string IssueStatusId { get; set; } = String.Empty;

        [Display(Name = "Type")]
        public string IssueTypeId { get; set; } = String.Empty;

        [ValidateNever]
        public List<EntityReference> IssueStatusList { get; set; } = new List<EntityReference>();

        [ValidateNever]
        public List<EntityReference> IssueTypeList { get; set; } = new List<EntityReference>();
    }
}

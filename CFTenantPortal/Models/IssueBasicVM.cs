using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Issue basic details for view model
    /// </summary>
    public class IssueBasicVM
    {
        public string Id { get; set; } = String.Empty;

        [Display(Name = "Description")]
        public string Description { get; set; } = String.Empty;

        [Display(Name = "Property/Building")]
        public string PropertyOrBuilderDescription { get; set; } = String.Empty;

        [Display(Name = "Type")]
        public string TypeDescription { get; set; } = String.Empty;

        [Display(Name = "Status")]
        public string StatusDescription { get; set; } = String.Empty;

        public string PropertyId { get; set; } = String.Empty;

        public string PropertyGroupId { get; set; } = String.Empty;
    }
}

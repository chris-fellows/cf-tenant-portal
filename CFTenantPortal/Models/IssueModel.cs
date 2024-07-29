using CFTenantPortal.Enums;
using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    // TODO: Do we need duplicate properties?
    public class IssueModel
    {
        public string Id { get; set; } = String.Empty;

        [Display(Name = "Description")]
        public string Description { get; set; } = String.Empty;

        [Display(Name = "Type")]
        public string IssueTypeId { get; set; } = String.Empty;

        [Display(Name = "Type")]
        public string IssueTypeDescription { get; set; } = String.Empty;

        [Display(Name = "Property")]
        public string PropertyDescription { get; set; } = String.Empty;

        [Display(Name = "Status")]
        public IssueStatuses Status { get; set; } = IssueStatuses.New;

        [Display(Name = "Status")]
        public string StatusDescription { get; set; } = String.Empty;

        public List<IssueTypeModel> IssueTypeList { get; set; }
    }
}

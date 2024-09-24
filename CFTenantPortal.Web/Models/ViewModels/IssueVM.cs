using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Issue view model
    /// </summary>
    public class IssueVM
    {
        public string Id { get; set; } = String.Empty;

        public string HeaderText { get; set; } = String.Empty;

        [Required]
        [Display(Name = "Reference")]
        public string Reference { get; set; } = String.Empty;

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; } = String.Empty;

        [Display(Name = "Type")]
        public string TypeId { get; set; } = String.Empty;

        [Display(Name = "Property")]
        public string PropertyId { get; set; } = String.Empty;

        [Display(Name = "Property Group")]
        public string PropertyGroupId { get; set; } = String.Empty;

        [Display(Name = "Status")]

        public string IssueStatusId { get; set; } = String.Empty;

        [Display(Name = "Created by Employee")]
        public string CreatedEmployeeId { get; set; } = String.Empty;

        [Display(Name = "Created by Owner")]
        public string CreatedPropertyOwnerId { get; set; } = String.Empty;

        public DocumentListVM DocumentList { get; set; } = new DocumentListVM();

        public MessageListVM MessageList { get; set; } = new MessageListVM();

        [ValidateNever]
        public List<EntityReference> IssueTypeList { get; set; } = new List<EntityReference>();

        [ValidateNever]
        public List<EntityReference> IssueStatusList { get; set; } = new List<EntityReference>();

        [ValidateNever]
        public List<EntityReference> PropertyList { get; set; } = new List<EntityReference>();

        [ValidateNever]
        public List<EntityReference> PropertyGroupList { get; set; } = new List<EntityReference>();

        [ValidateNever]
        public List<EntityReference> EmployeeList { get; set; } = new List<EntityReference>();

        [ValidateNever]
        public List<EntityReference> PropertyOwnerList { get; set; } = new List<EntityReference>();

        public bool AllowSave { get; set; }
    }
}

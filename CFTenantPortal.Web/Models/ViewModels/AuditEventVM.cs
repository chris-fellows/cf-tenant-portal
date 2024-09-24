using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;

namespace CFTenantPortal.Models
{
    public class AuditEventVM
    {
        public string Id { get; set; }

        public string HeaderText { get; set; } = String.Empty;

        [Display(Name = "Type")]
        public string TypeDescription { get; set; } = String.Empty;

        public List<AuditEventParameterVM> Parameters { get; set; }

        //public string DocumentId { get; set; } = String.Empty;

        //[Display(Name = "Document")]
        //public string DocumentName { get; set; } = String.Empty;

        //public string IssueId { get; set; } = String.Empty;

        //[Display(Name = "Issue")]
        //public string IssueRef { get; set; } = String.Empty;

        //public string PropertyId { get; set; } = String.Empty;

        //[Display(Name = "Property")]
        //public string PropertyDescription { get; set; } = String.Empty;

        //public string PropertyOwnerId { get; set; } = String.Empty;

        //[Display(Name = "Owner")]
        //public string PropertyOwnerDescription { get; set; } = String.Empty;

        //public string PropertyGroupId { get; set; } = String.Empty;

        //[Display(Name = "Property Group")]
        //public string PropertyGroupDescription { get; set; } = String.Empty;

        [Display(Name = "Created")]
        public DateTimeOffset CreatedDateTime { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Property group view m
    /// </summary>
    public class PropertyGroupVM
    {
        public string Id { get; set; } = String.Empty;

        public string HeaderText = String.Empty;

        [Required]
        [MaxLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; } = String.Empty;

        [Required]
        [MaxLength(200)]
        [Display(Name = "Description")]
        public string Description { get; set; } = String.Empty;

        //public List<PropertyBasicVM> Properties { get; set; } = new List<PropertyBasicVM>();
        public PropertyListVM PropertyList { get; set; } = new PropertyListVM();

        //public List<DocumentBasicVM> Documents { get; set; } = new List<DocumentBasicVM>();
        public DocumentListVM DocumentList { get; set; } = new DocumentListVM();

        //public List<IssueBasicVM> Issues { get; set; } = new List<IssueBasicVM>();
        public IssueListVM IssueList { get; set; } = new IssueListVM();

        public bool AllowSave { get; set; }

        //public bool AllowDelete { get; set; }
    }
}

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

        [Display(Name = "Name")]
        public string Name { get; set; } = String.Empty;

        [Display(Name = "Description")]
        public string Description { get; set; } = String.Empty;

        public List<PropertyBasicVM> Properties { get; set; } = new List<PropertyBasicVM>();

        public List<IssueBasicVM> Issues { get; set; } = new List<IssueBasicVM>();
    }
}

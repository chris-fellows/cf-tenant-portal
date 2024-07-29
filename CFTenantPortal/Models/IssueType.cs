using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Issue type
    /// </summary>
    public class IssueType
    {
        public string Id { get; set; } = String.Empty;

        [Display(Name = "Description")]
        public string Description { get; set; } = String.Empty;
    }
}

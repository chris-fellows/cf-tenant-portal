using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    public class PropertyGroupBasicVM
    {
        public string Id { get; set; } = String.Empty;
        
        [Display(Name = "Name")]
        public string Name { get; set; } = String.Empty;
        
        [Display(Name = "Description")]
        public string Description { get; set; } = String.Empty;

        public bool AllowDelete { get; set; }
    }
}

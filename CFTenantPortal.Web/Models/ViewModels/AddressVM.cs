using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Address view model
    /// </summary>
    public class AddressVM
    {
        [Display(Name = "Line 1")]
        public string Line1 { get; set; } = String.Empty;

        [Display(Name = "Line 2")]
        public string Line2 { get; set; } = String.Empty;

        [Display(Name = "Town")]
        public string Town { get; set; } = String.Empty;

        [Display(Name = "County")]
        public string County { get; set; } = String.Empty;

        [Display(Name = "Postcode")]
        public string Postcode { get; set; } = String.Empty;
    }
}

using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Address view model
    /// </summary>
    public class AddressVM
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "Line 1")]
        public string Line1 { get; set; } = String.Empty;

        [MaxLength(100)]
        [Display(Name = "Line 2")]
        public string Line2 { get; set; } = String.Empty;

        [Required]
        [MaxLength(50)]
        [Display(Name = "Town")]
        public string Town { get; set; } = String.Empty;

        [Required]
        [MaxLength(50)]
        [Display(Name = "County")]
        public string County { get; set; } = String.Empty;

        // https://www.oreilly.com/library/view/regular-expressions-cookbook/9781449327453/ch04s16.html
        [Required]
        [MaxLength(15)]
        //[RegularExpression(pattern: "^[A-Z]{1,2}[0-9R][0-9A-Z]?●[0-9][ABD-HJLNP-UW-Z]{2}$", ErrorMessage = "Postcode is invalid")]
        [Display(Name = "Postcode")]
        public string Postcode { get; set; } = String.Empty;
    }
}

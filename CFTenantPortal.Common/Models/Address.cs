using System.Text;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Address
    /// </summary>
    public class Address
    {
        public string Line1 { get; set; } = String.Empty;

        public string Line2 { get; set; } = String.Empty;

        public string Town { get; set; } = String.Empty;

        public string County { get; set; } = String.Empty;

        public string Postcode { get; set; } = String.Empty;        

        public string ToSummary()
        {
            var line = new StringBuilder(Line1);

            if (!String.IsNullOrEmpty(Town))
            {
                line.Append($", {Town}");
            }
            if (!String.IsNullOrEmpty(County))
            {
                line.Append($", {County}");
            }
            if (!String.IsNullOrEmpty(Postcode))
            {
                line.Append($", {Postcode}");
            }

            return line.ToString();
        }
    }
}

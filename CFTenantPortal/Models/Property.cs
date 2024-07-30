namespace CFTenantPortal.Models
{
    /// <summary>
    /// Property. E.g. Flat
    /// </summary>
    public class Property
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Property owner
        /// </summary>
        public string OwnerId { get; set; } = String.Empty;

        /// <summary>
        /// Property group
        /// </summary>
        public string GroupId { get; set; } = String.Empty;

        /// <summary>
        /// Property address
        /// </summary>
        public Address Address { get; set; }        

        /// <summary>
        /// Property features. E.g. Allocated parking space.
        /// </summary>
        public List<string> FeatureTypeIds { get; set; }
    }
}

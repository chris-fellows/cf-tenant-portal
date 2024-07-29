namespace CFTenantPortal.Models
{
    /// <summary>
    /// Property owner
    /// </summary>
    public class PropertyOwner
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Owner name
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Email address
        /// </summary>
        public string Email { get; set; } = String.Empty;

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; } = String.Empty;            
    }
}

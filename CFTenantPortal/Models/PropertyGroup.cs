namespace CFTenantPortal.Models
{
    /// <summary>
    /// Group of properties. E.g. Build for multiple flats.
    /// </summary>
    public class PropertyGroup
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        public string Id { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;
    }
}

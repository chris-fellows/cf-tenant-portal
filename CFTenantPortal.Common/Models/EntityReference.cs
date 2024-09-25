namespace CFTenantPortal.Models
{
    /// <summary>
    /// Reference to entity. E.g. Property, property group, property owner etc
    /// </summary>
    public class EntityReference
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Returns reference for None. This is used for optional properties
        /// </summary>
        public static EntityReference None
        {
            get
            {
                return new EntityReference() { Id = "None", Name = "None" };
            }
        }
    }
}

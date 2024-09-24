namespace CFTenantPortal.Models
{
    public class AuditEventParameterVM
    {
        /// <summary>
        /// Entity type description (First parameter)
        /// </summary>
        public string EntityTypeDescription { get; set; }

        /// <summary>
        /// Entity Id (E.g. Document Id, Issue Id, Property Id etc) for first parameter
        /// </summary>
        public string EntityId { get; set; } = String.Empty;

        /// <summary>
        /// Entity description (E.g. Document name, Issue ref, Property address etc) for first parameter
        /// </summary>
        public string EntityDescription { get; set; } = String.Empty;

        /// <summary>
        /// MVC route for displaying entity detail for first parameter
        /// </summary>
        public string EntityDetailRoute { get; set; } = String.Empty;
    }
}

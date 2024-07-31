namespace CFTenantPortal.Models
{
    /// <summary>
    /// Message template
    /// </summary>
    public class MessageTemplate
    {
        public string Id { get; set; } = String.Empty;

        public string MessageTypeId { get; set; } = String.Empty;

        /// <summary>
        /// Template text. May contain placeholders that are replaced at runtime.
        /// </summary>
        public string Text { get; set; } = String.Empty;
    }
}

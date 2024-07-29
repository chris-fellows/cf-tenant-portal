namespace CFTenantPortal.Models
{
    /// <summary>
    /// Message for property owner
    /// </summary>
    public class Message
    {
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Message type
        /// </summary>
        public string MessageTypeId { get; set; } = String.Empty;

        /// <summary>
        /// Recipient owner
        /// </summary>
        public string PropertyOwnerId { get; set; } = String.Empty;

        /// <summary>
        /// Property (if any) that message relates to. It's possible to send the message to the owner for
        /// all properties.
        /// </summary>
        public string PropertyId { get; set; } = String.Empty;

        public string Text { get; set; } = String.Empty;

        /// <summary>
        /// Documents
        /// </summary>
        public List<string> DocumentIds { get; set; }

        public DateTimeOffset CreatedDateTime { get; set; } = DateTimeOffset.Now;
    }
}

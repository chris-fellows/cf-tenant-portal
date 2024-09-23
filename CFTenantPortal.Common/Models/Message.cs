using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Message for property owner.
    /// 
    /// Message can relate to the following:
    /// - Property.
    /// - Issue. E.g. Notification of status changed.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
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
        /// Issue (if any) that message relates to. E.g. Progress notification.
        /// </summary>
        public string IssueId { get; set; } = String.Empty;

        /// <summary>
        /// Property (if any) that message relates to
        /// </summary>
        public string PropertyId { get; set; } = String.Empty;

        /// <summary>
        /// Message text
        /// </summary>
        public string Text { get; set; } = String.Empty;

        /// <summary>
        /// Documents. E.g. Invoice for management fees.
        /// </summary>
        public List<string> DocumentIds { get; set; } = new List<string>();

        public DateTimeOffset CreatedDateTime { get; set; } = DateTimeOffset.Now;
    }
}

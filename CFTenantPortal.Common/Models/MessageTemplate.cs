using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Message template
    /// </summary>
    public class MessageTemplate
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        public string MessageTypeId { get; set; } = String.Empty;

        /// <summary>
        /// Template text. May contain placeholders that are replaced at runtime.
        /// </summary>
        public string Text { get; set; } = String.Empty;
    }
}

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Message type
    /// </summary>
    public class MessageType
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// Default template to use (MessageTemplate.Id)
        /// </summary>
        public string DefaultTemplateId { get; set; } = String.Empty;
    }
}

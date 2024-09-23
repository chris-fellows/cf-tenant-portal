using CFTenantPortal.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CFTenantPortal.Models
{
    public class AuditEventType
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// Audit event type enum. Typically for finding a specific AuditEventType instance where the Id
        /// isn't known
        /// </summary>
        public AuditEventTypes EventType { get; set; }
    }
}

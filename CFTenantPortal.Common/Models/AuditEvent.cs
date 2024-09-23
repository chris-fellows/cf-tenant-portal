using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Audit event
    /// </summary>
    public class AuditEvent
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        public string EventTypeId { get; set; } = String.Empty;

        public DateTimeOffset CreatedDateTime { get; set; } = DateTimeOffset.Now;

        public List<AuditEventParameter> Parameters { get; set; } = new List<AuditEventParameter>();
    }
}

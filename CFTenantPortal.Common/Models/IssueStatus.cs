using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Security.Permissions;

namespace CFTenantPortal.Models
{
    public class IssueStatus
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;
    }
}

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Issue type
    /// </summary>
    public class IssueType
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;
        
        public string Description { get; set; } = String.Empty;
    }
}

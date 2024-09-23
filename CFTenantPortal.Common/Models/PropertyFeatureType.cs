using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Property feature type.
    /// </summary>
    public class PropertyFeatureType
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;
    }
}

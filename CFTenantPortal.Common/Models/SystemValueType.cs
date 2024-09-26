using CFTenantPortal.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// System value type    
    /// </summary>
    public class SystemValueType
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// System value type enum. Typically for finding a specific SystemValueType instance where the Id
        /// isn't known
        /// </summary>
        public SystemValueTypes ValueType { get; set; }
    }
}

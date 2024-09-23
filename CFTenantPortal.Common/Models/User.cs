using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CFTenantPortal.Models
{
    public class User
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;

        public string Email { get; set; }

        public bool Active { get; set; } = true;
    }
}

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Property. E.g. Flat
    /// </summary>
    public class Property
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Property owner
        /// </summary>
        public string OwnerId { get; set; } = String.Empty;

        /// <summary>
        /// Property group
        /// </summary>
        public string GroupId { get; set; } = String.Empty;

        /// <summary>
        /// Property address
        /// </summary>
        public Address Address { get; set; } = new Address();

        /// <summary>
        /// Documents associated with property
        /// </summary>
        public List<string> DocumentIds { get; set; } = new List<string>();

        /// <summary>
        /// Property features. E.g. Allocated parking space.
        /// </summary>
        public List<string>? FeatureTypeIds { get; set; }
    }
}
